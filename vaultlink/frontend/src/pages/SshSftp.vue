<template>
  <div class="min-h-screen bg-gradient-to-br from-purple-50 via-white to-pink-50">
    <!-- Header -->
    <div class="bg-gradient-to-r from-purple-600 to-pink-600 text-white">
      <div class="max-w-6xl mx-auto px-6 py-12">
        <div class="flex items-center">
          <div class="bg-white/10 p-3 rounded-2xl mr-4">
            <svg class="w-10 h-10" fill="currentColor" viewBox="0 0 24 24">
              <path d="M19.35 10.04A7.49 7.49 0 0 0 12 4C9.11 4 6.6 5.64 5.35 8.04A5.994 5.994 0 0 0 0 14c0 3.31 2.69 6 6 6h13c2.76 0 5-2.24 5-5 0-2.64-2.05-4.78-4.65-4.96z"/>
            </svg>
          </div>
          <div>
            <h1 class="text-3xl font-bold">Quản lý SSH/SFTP</h1>
            <p class="text-purple-100 mt-2">Kết nối và truyền file một cách an toàn</p>
          </div>
        </div>
      </div>
    </div>

    <div class="max-w-6xl mx-auto px-6 py-8 space-y-8">
      <!-- Connection Settings -->
      <div class="bg-white rounded-2xl shadow-lg p-8">
        <div class="flex items-center mb-6">
          <div class="bg-blue-100 p-3 rounded-xl mr-4">
            <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 24 24">
              <path d="M15,9H9V7.5C9,6.12 10.12,5 11.5,5C12.88,5 14,6.12 14,7.5V9M21,16V12C21,10.89 20.11,10 19,10H18V7.5C18,4.46 15.54,2 12.5,2C9.46,2 7,4.46 7,7.5V10H6C4.89,10 4,10.89 4,12V16C4,17.11 4.89,18 6,18H19C20.11,18 21,17.11 21,16Z"/>
            </svg>
          </div>
          <div>
            <h3 class="text-xl font-bold text-gray-800">Cấu hình kết nối</h3>
            <p class="text-gray-600">Thiết lập thông tin SSH/SFTP server</p>
          </div>
        </div>
        
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">Host</label>
            <input
              v-model="connection.host"
              type="text"
              placeholder="localhost"
              class="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-purple-500 focus:border-transparent"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">Port</label>
            <input
              v-model.number="connection.port"
              type="number"
              placeholder="2222"
              class="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-purple-500 focus:border-transparent"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">User</label>
            <input
              v-model="connection.user"
              type="text"
              placeholder="demo"
              class="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-purple-500 focus:border-transparent"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">SSH Key Path</label>
            <input
              v-model="connection.privateKeyPath"
              type="text"
              placeholder="ops/ssh/keys/demo_ed25519"
              class="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-purple-500 focus:border-transparent"
            />
          </div>
        </div>
        
        <div class="flex space-x-4">
          <button
            @click="testConnection"
            :disabled="loading"
            class="inline-flex items-center px-6 py-3 bg-blue-600 text-white font-medium rounded-xl hover:bg-blue-700 transition-colors disabled:opacity-50"
          >
            <svg v-if="loading" class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            {{ loading ? 'Đang kết nối...' : '🔌 Test kết nối' }}
          </button>
          
          <button
            @click="listFiles"
            :disabled="loading"
            class="inline-flex items-center px-6 py-3 bg-purple-600 text-white font-medium rounded-xl hover:bg-purple-700 transition-colors disabled:opacity-50"
          >
            {{ loading ? 'Đang tải...' : '📂 Liệt kê file' }}
          </button>
        </div>
        
        <!-- Connection Status -->
        <div v-if="connectionStatus" class="mt-6 p-4 rounded-xl" :class="connectionStatus.success ? 'bg-green-50 border border-green-200' : 'bg-red-50 border border-red-200'">
          <div class="flex items-center">
            <span class="mr-2">{{ connectionStatus.success ? '✅' : '❌' }}</span>
            <span :class="connectionStatus.success ? 'text-green-800' : 'text-red-800'">
              {{ connectionStatus.message }}
            </span>
          </div>
        </div>
      </div>

      <!-- File Operations -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
        <!-- Upload Section -->
        <div class="bg-white rounded-2xl shadow-lg p-8">
          <div class="flex items-center mb-6">
            <div class="bg-green-100 p-3 rounded-xl mr-4">
              <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 24 24">
                <path d="M14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2M18,20H6V4H13V9H18V20Z"/>
              </svg>
            </div>
            <div>
              <h3 class="text-xl font-bold text-gray-800">Upload file</h3>
              <p class="text-gray-600">Tải file lên SFTP server</p>
            </div>
          </div>
          
          <div class="space-y-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Chọn file để upload</label>
              <div class="border-2 border-dashed border-gray-300 rounded-xl p-6 text-center hover:border-green-400 transition-colors">
                <input
                  ref="uploadFileInput"
                  type="file"
                  @change="handleUploadFileSelect"
                  class="hidden"
                />
                <button @click="$refs.uploadFileInput.click()" class="text-green-600 hover:text-green-700">
                  📁 {{ selectedUploadFile ? selectedUploadFile.name : 'Chọn file để upload' }}
                </button>
              </div>
            </div>
            
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Đường dẫn remote (tùy chọn)</label>
              <input
                v-model="uploadPath"
                type="text"
                placeholder="upload/"
                class="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-green-500 focus:border-transparent"
              />
            </div>
            
            <button
              @click="uploadFile"
              :disabled="loading || !selectedUploadFile"
              class="w-full inline-flex items-center justify-center px-6 py-3 bg-green-600 text-white font-medium rounded-xl hover:bg-green-700 transition-colors disabled:opacity-50"
            >
              {{ loading ? 'Đang upload...' : '⬆️ Upload file' }}
            </button>
          </div>
        </div>

        <!-- Download Section -->
        <div class="bg-white rounded-2xl shadow-lg p-8">
          <div class="flex items-center mb-6">
            <div class="bg-orange-100 p-3 rounded-xl mr-4">
              <svg class="w-6 h-6 text-orange-600" fill="currentColor" viewBox="0 0 24 24">
                <path d="M5,20H19V18H5M19,9H15V3H9V9H5L12,16L19,9Z"/>
              </svg>
            </div>
            <div>
              <h3 class="text-xl font-bold text-gray-800">Download file</h3>
              <p class="text-gray-600">Tải file từ SFTP server</p>
            </div>
          </div>
          
          <div class="space-y-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Đường dẫn file remote</label>
              <input
                v-model="downloadPath"
                type="text"
                placeholder="upload/filename.txt"
                class="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-orange-500 focus:border-transparent"
              />
            </div>
            
            <button
              @click="downloadFile"
              :disabled="loading || !downloadPath"
              class="w-full inline-flex items-center justify-center px-6 py-3 bg-orange-600 text-white font-medium rounded-xl hover:bg-orange-700 transition-colors disabled:opacity-50"
            >
              {{ loading ? 'Đang download...' : '⬇️ Download file' }}
            </button>
          </div>
        </div>
      </div>

      <!-- File List -->
      <div v-if="fileList.length > 0" class="bg-white rounded-2xl shadow-lg p-8">
        <div class="flex items-center justify-between mb-6">
          <div>
            <h3 class="text-xl font-bold text-gray-800">Danh sách file</h3>
            <p class="text-gray-600">File trên SFTP server</p>
          </div>
          <button @click="listFiles" class="text-purple-600 hover:text-purple-700">
            🔄 Tải lại
          </button>
        </div>
        
        <div class="overflow-hidden">
          <div class="grid grid-cols-1 gap-3">
            <div v-for="file in fileList" :key="file.name" class="flex items-center justify-between p-4 border border-gray-200 rounded-xl hover:bg-gray-50 transition-colors">
              <div class="flex items-center">
                <span class="text-2xl mr-3">{{ file.isDirectory ? '📁' : '📄' }}</span>
                <div>
                  <div class="font-medium text-gray-900">{{ file.name }}</div>
                  <div class="text-sm text-gray-500">
                    {{ file.isDirectory ? 'Thư mục' : `${formatFileSize(file.size)} - ${formatDate(file.modified)}` }}
                  </div>
                </div>
              </div>
              
              <div v-if="!file.isDirectory" class="flex space-x-2">
                <button
                  @click="downloadPath = file.fullPath || file.name"
                  class="px-3 py-1 text-sm bg-orange-100 text-orange-700 rounded-lg hover:bg-orange-200 transition-colors"
                >
                  📥 Chọn
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Quick Demo Section -->
      <div class="bg-gradient-to-r from-blue-100 to-purple-100 rounded-2xl p-8">
        <div class="text-center">
          <h3 class="text-xl font-bold text-gray-800 mb-4">🚀 Demo nhanh</h3>
          <p class="text-gray-600 mb-6">Thử nghiệm quy trình hoàn chỉnh PGP + SFTP</p>
          
          <div class="grid grid-cols-1 md:grid-cols-3 gap-4 text-sm">
            <div class="bg-white rounded-xl p-4">
              <div class="text-2xl mb-2">1️⃣</div>
              <div class="font-medium">Mã hóa file ở tab PGP</div>
            </div>
            <div class="bg-white rounded-xl p-4">
              <div class="text-2xl mb-2">2️⃣</div>
              <div class="font-medium">Upload file đã mã hóa</div>
            </div>
            <div class="bg-white rounded-xl p-4">
              <div class="text-2xl mb-2">3️⃣</div>
              <div class="font-medium">Download và giải mã</div>
            </div>
          </div>
          
          <router-link to="/pgp" class="inline-flex items-center mt-6 px-6 py-3 bg-blue-600 text-white font-medium rounded-xl hover:bg-blue-700 transition-colors">
            🔐 Đi tới PGP
          </router-link>
        </div>
      </div>
    </div>

    <!-- Messages -->
    <div v-if="message" class="fixed bottom-4 right-4 max-w-md">
      <div class="rounded-xl p-4 shadow-lg" :class="messageType === 'error' ? 'bg-red-100 text-red-800 border border-red-200' : 'bg-green-100 text-green-800 border border-green-200'">
        <div class="flex items-center">
          <span class="mr-2">{{ messageType === 'error' ? '❌' : '✅' }}</span>
          {{ message }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
const API_BASE = import.meta.env.VITE_API_BASE || 'http://localhost:5080'

export default {
  name: 'SshSftp',
  data() {
    return {
      loading: false,
      message: '',
      messageType: 'success',
      connectionStatus: null,
      fileList: [],
      selectedUploadFile: null,
      uploadPath: 'upload/',
      downloadPath: '',
      connection: {
        host: 'localhost',
        port: 2222,
        user: 'demo',
        privateKeyPath: 'ops/ssh/keys/demo_ed25519'
      }
    }
  },
  methods: {
    async testConnection() {
      this.loading = true
      this.connectionStatus = null
      
      try {
        const response = await fetch(`${API_BASE}/ssh/test`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(this.connection)
        })

        const result = await response.json()
        
        this.connectionStatus = {
          success: response.ok,
          message: result.message || (response.ok ? 'Kết nối SSH thành công!' : 'Kết nối SSH thất bại')
        }
        
        if (response.ok) {
          this.showMessage('Kết nối SSH thành công!')
        } else {
          this.showMessage(result.error || 'Kết nối SSH thất bại', 'error')
        }
      } catch (error) {
        this.connectionStatus = {
          success: false,
          message: 'Lỗi kết nối: ' + error.message
        }
        this.showMessage('Lỗi: ' + error.message, 'error')
      } finally {
        this.loading = false
      }
    },
    
    async listFiles() {
      this.loading = true
      
      try {
        const response = await fetch(`${API_BASE}/sftp/list`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            ...this.connection,
            remotePath: this.uploadPath || '/'
          })
        })

        if (response.ok) {
          this.fileList = await response.json()
          this.showMessage(`Tìm thấy ${this.fileList.length} item`)
        } else {
          const error = await response.json()
          this.showMessage(error.error || 'Liệt kê file thất bại', 'error')
        }
      } catch (error) {
        this.showMessage('Lỗi: ' + error.message, 'error')
      } finally {
        this.loading = false
      }
    },
    
    handleUploadFileSelect(event) {
      this.selectedUploadFile = event.target.files[0]
    },
    
    async uploadFile() {
      if (!this.selectedUploadFile) {
        this.showMessage('Vui lòng chọn file để upload', 'error')
        return
      }

      this.loading = true
      const formData = new FormData()
      formData.append('file', this.selectedUploadFile)
      formData.append('host', this.connection.host)
      formData.append('port', this.connection.port.toString())
      formData.append('user', this.connection.user)
      formData.append('privateKeyPath', this.connection.privateKeyPath)
      formData.append('remotePath', this.uploadPath + this.selectedUploadFile.name)

      try {
        const response = await fetch(`${API_BASE}/sftp/upload`, {
          method: 'POST',
          body: formData
        })

        if (response.ok) {
          this.showMessage('Upload file thành công!')
          this.selectedUploadFile = null
          this.$refs.uploadFileInput.value = ''
          await this.listFiles()
        } else {
          const error = await response.json()
          this.showMessage(error.error || 'Upload file thất bại', 'error')
        }
      } catch (error) {
        this.showMessage('Lỗi: ' + error.message, 'error')
      } finally {
        this.loading = false
      }
    },
    
    async downloadFile() {
      if (!this.downloadPath) {
        this.showMessage('Vui lòng nhập đường dẫn file', 'error')
        return
      }

      this.loading = true
      
      try {
        const response = await fetch(`${API_BASE}/sftp/download`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            ...this.connection,
            remotePath: this.downloadPath
          })
        })

        if (response.ok) {
          const blob = await response.blob()
          const url = window.URL.createObjectURL(blob)
          const a = document.createElement('a')
          a.href = url
          a.download = this.downloadPath.split('/').pop() || 'download'
          a.click()
          window.URL.revokeObjectURL(url)
          this.showMessage('Download file thành công!')
        } else {
          const error = await response.json()
          this.showMessage(error.error || 'Download file thất bại', 'error')
        }
      } catch (error) {
        this.showMessage('Lỗi: ' + error.message, 'error')
      } finally {
        this.loading = false
      }
    },
    
    formatFileSize(bytes) {
      if (bytes === 0) return '0 Bytes'
      const k = 1024
      const sizes = ['Bytes', 'KB', 'MB', 'GB']
      const i = Math.floor(Math.log(bytes) / Math.log(k))
      return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i]
    },
    
    formatDate(dateString) {
      return new Date(dateString).toLocaleDateString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
      })
    },
    
    showMessage(text, type = 'success') {
      this.message = text
      this.messageType = type
      setTimeout(() => {
        this.message = ''
      }, 5000)
    }
  }
}
</script>