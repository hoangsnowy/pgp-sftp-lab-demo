<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900">
    <!-- Animated Background -->
    <div class="absolute inset-0 overflow-hidden">
      <div class="absolute -inset-10 opacity-30">
        <div class="absolute top-0 -left-4 w-72 h-72 bg-purple-300 rounded-full mix-blend-multiply filter blur-xl opacity-70 animate-blob"></div>
        <div class="absolute top-0 -right-4 w-72 h-72 bg-pink-300 rounded-full mix-blend-multiply filter blur-xl opacity-70 animate-blob animation-delay-2000"></div>
        <div class="absolute -bottom-8 left-20 w-72 h-72 bg-cyan-300 rounded-full mix-blend-multiply filter blur-xl opacity-70 animate-blob animation-delay-4000"></div>
      </div>
    </div>

    <!-- Header -->
    <div class="relative bg-gradient-to-r from-purple-600/80 to-pink-600/80 backdrop-blur-sm border-b border-white/10">
      <div class="max-w-6xl mx-auto px-6 py-12">
        <div class="flex items-center">
          <div class="bg-white/10 backdrop-blur-sm p-3 rounded-2xl mr-4 border border-white/20">
            <svg class="w-10 h-10 text-white" fill="currentColor" viewBox="0 0 24 24">
              <path d="M19.35 10.04A7.49 7.49 0 0 0 12 4C9.11 4 6.6 5.64 5.35 8.04A5.994 5.994 0 0 0 0 14c0 3.31 2.69 6 6 6h13c2.76 0 5-2.24 5-5 0-2.64-2.05-4.78-4.65-4.96z"/>
            </svg>
          </div>
          <div>
            <h1 class="text-3xl font-bold text-white">Quản lý SSH/SFTP</h1>
            <p class="text-purple-200 mt-2">Kết nối và truyền file một cách an toàn</p>
          </div>
        </div>
      </div>
    </div>

    <div class="relative max-w-6xl mx-auto px-6 py-8 space-y-8">
      <!-- Connection Settings -->
      <div class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-2xl shadow-lg p-8">
        <div class="flex items-center mb-6">
          <div class="bg-blue-500/20 backdrop-blur-sm border border-blue-400/30 p-3 rounded-xl mr-4">
            <svg class="w-6 h-6 text-blue-400" fill="currentColor" viewBox="0 0 24 24">
              <path d="M15,9H9V7.5C9,6.12 10.12,5 11.5,5C12.88,5 14,6.12 14,7.5V9M21,16V12C21,10.89 20.11,10 19,10H18V7.5C18,4.46 15.54,2 12.5,2C9.46,2 7,4.46 7,7.5V10H6C4.89,10 4,10.89 4,12V16C4,17.11 4.89,18 6,18H19C20.11,18 21,17.11 21,16Z"/>
            </svg>
          </div>
          <div>
            <h3 class="text-xl font-bold text-white">Cấu hình kết nối</h3>
            <p class="text-gray-300">Thiết lập thông tin SSH/SFTP server</p>
          </div>
        </div>
        
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-6">
          <div>
            <label class="block text-sm font-medium text-gray-300 mb-2">Host</label>
            <input
              v-model="connection.host"
              type="text"
              placeholder="localhost"
              class="w-full px-4 py-3 bg-white/5 border border-white/10 text-white placeholder-gray-400 rounded-xl focus:ring-2 focus:ring-purple-500 focus:border-transparent backdrop-blur-sm"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-300 mb-2">Port</label>
            <input
              v-model.number="connection.port"
              type="number"
              placeholder="2222"
              class="w-full px-4 py-3 bg-white/5 border border-white/10 text-white placeholder-gray-400 rounded-xl focus:ring-2 focus:ring-purple-500 focus:border-transparent backdrop-blur-sm"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-300 mb-2">User</label>
            <input
              v-model="connection.user"
              type="text"
              placeholder="demo"
              class="w-full px-4 py-3 bg-white/5 border border-white/10 text-white placeholder-gray-400 rounded-xl focus:ring-2 focus:ring-purple-500 focus:border-transparent backdrop-blur-sm"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-300 mb-2">Password</label>
            <input
              v-model="connection.password"
              type="password"
              placeholder="pass"
              class="w-full px-4 py-3 bg-white/5 border border-white/10 text-white placeholder-gray-400 rounded-xl focus:ring-2 focus:ring-purple-500 focus:border-transparent backdrop-blur-sm"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-300 mb-2">SSH Key Path (tùy chọn)</label>
            <input
              v-model="connection.privateKeyPath"
              type="text"
              placeholder="Để trống nếu dùng password"
              class="w-full px-4 py-3 bg-white/5 border border-white/10 text-white placeholder-gray-400 rounded-xl focus:ring-2 focus:ring-purple-500 focus:border-transparent backdrop-blur-sm"
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
        <div v-if="connectionStatus" class="mt-6 p-4 rounded-xl backdrop-blur-sm" :class="connectionStatus.success ? 'bg-green-500/20 border border-green-400/50' : 'bg-red-500/20 border border-red-400/50'">
          <div class="flex items-center">
            <span class="mr-2">{{ connectionStatus.success ? '✅' : '❌' }}</span>
            <span :class="connectionStatus.success ? 'text-green-300' : 'text-red-300'">
              {{ connectionStatus.message }}
            </span>
          </div>
        </div>
      </div>

      <!-- File Operations -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
        <!-- Upload Section -->
        <div class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-2xl shadow-lg p-8">
          <div class="flex items-center mb-6">
            <div class="bg-green-500/20 backdrop-blur-sm border border-green-400/30 p-3 rounded-xl mr-4">
              <svg class="w-6 h-6 text-green-400" fill="currentColor" viewBox="0 0 24 24">
                <path d="M14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2M18,20H6V4H13V9H18V20Z"/>
              </svg>
            </div>
            <div>
              <h3 class="text-xl font-bold text-white">Upload file</h3>
              <p class="text-gray-300">Tải file lên SFTP server</p>
            </div>
          </div>
          
          <div class="space-y-6">
            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Chọn file để upload</label>
              <div class="border-2 border-dashed border-white/20 rounded-xl p-6 text-center hover:border-green-400/50 transition-colors bg-white/5 backdrop-blur-sm">
                <input
                  ref="uploadFileInput"
                  type="file"
                  @change="handleUploadFileSelect"
                  class="hidden"
                />
                <button @click="$refs.uploadFileInput.click()" class="text-green-400 hover:text-green-300 transition-colors">
                  📁 {{ selectedUploadFile ? selectedUploadFile.name : 'Chọn file để upload' }}
                </button>
              </div>
            </div>
            
            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Đường dẫn remote (tùy chọn)</label>
              <input
                v-model="uploadPath"
                type="text"
                placeholder="upload/"
                class="w-full px-4 py-3 bg-white/5 border border-white/10 text-white placeholder-gray-400 rounded-xl focus:ring-2 focus:ring-green-500 focus:border-transparent backdrop-blur-sm"
              />
            </div>
            
            <button
              @click="uploadFile"
              :disabled="loading || !selectedUploadFile"
              class="w-full inline-flex items-center justify-center px-6 py-3 bg-gradient-to-r from-green-500 to-green-600 text-white font-medium rounded-xl hover:from-green-400 hover:to-green-500 transform hover:scale-105 transition-all duration-300 disabled:opacity-50 disabled:transform-none"
            >
              {{ loading ? 'Đang upload...' : '⬆️ Upload file' }}
            </button>
          </div>
        </div>

        <!-- Download Section -->
        <div class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-2xl shadow-lg p-8">
          <div class="flex items-center mb-6">
            <div class="bg-orange-500/20 backdrop-blur-sm border border-orange-400/30 p-3 rounded-xl mr-4">
              <svg class="w-6 h-6 text-orange-400" fill="currentColor" viewBox="0 0 24 24">
                <path d="M5,20H19V18H5M19,9H15V3H9V9H5L12,16L19,9Z"/>
              </svg>
            </div>
            <div>
              <h3 class="text-xl font-bold text-white">Download file</h3>
              <p class="text-gray-300">Tải file từ SFTP server</p>
            </div>
          </div>
          
          <div class="space-y-6">
            <div>
              <label class="block text-sm font-medium text-gray-300 mb-2">Đường dẫn file remote</label>
              <input
                v-model="downloadPath"
                type="text"
                placeholder="upload/filename.txt"
                class="w-full px-4 py-3 bg-white/5 border border-white/10 text-white placeholder-gray-400 rounded-xl focus:ring-2 focus:ring-orange-500 focus:border-transparent backdrop-blur-sm"
              />
              <div v-if="downloadPath" class="mt-2 text-sm text-gray-400">
                <div>📁 Current path: {{ currentPath }}</div>
                <div>📄 Selected file: {{ downloadPath }}</div>
                <div>🔗 Full path: {{ currentPath.endsWith('/') ? currentPath + downloadPath.replace(/^\/+/, '') : currentPath + '/' + downloadPath.replace(/^\/+/, '') }}</div>
              </div>
            </div>
            
            <button
              @click="downloadFile"
              :disabled="loading || !downloadPath"
              class="w-full inline-flex items-center justify-center px-6 py-3 bg-gradient-to-r from-orange-500 to-orange-600 text-white font-medium rounded-xl hover:from-orange-400 hover:to-orange-500 transform hover:scale-105 transition-all duration-300 disabled:opacity-50 disabled:transform-none"
            >
              {{ loading ? 'Đang download...' : '⬇️ Download file' }}
            </button>
          </div>
        </div>
      </div>

      <!-- File List -->
      <div v-if="fileList.length > 0" class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-2xl shadow-lg p-8">
        <div class="flex items-center justify-between mb-6">
          <div>
            <h3 class="text-xl font-bold text-white">Danh sách file</h3>
            <p class="text-gray-300">File trên SFTP server</p>
          </div>
          <button @click="listFiles" class="text-purple-400 hover:text-purple-300 transition-colors">
            🔄 Tải lại
          </button>
        </div>
        
        <div class="overflow-hidden">
          <div class="grid grid-cols-1 gap-3">
            <div v-for="file in fileList" :key="file.name" class="flex items-center justify-between p-4 border border-white/10 rounded-xl hover:bg-white/5 transition-colors backdrop-blur-sm">
              <div class="flex items-center">
                <span class="text-2xl mr-3">{{ file.isDirectory ? '📁' : '📄' }}</span>
                <div>
                  <div class="font-medium text-white">{{ file.name }}</div>
                  <div class="text-sm text-gray-400">
                    {{ file.isDirectory ? 'Thư mục' : `${formatFileSize(file.size)} - ${formatDate(file.modified)}` }}
                  </div>
                </div>
              </div>
              
              <div v-if="!file.isDirectory || file.name.includes('.')" class="flex space-x-2">
                <button
                  @click="directDownloadFile(file)"
                  class="px-3 py-1 text-sm bg-green-500/20 text-green-400 border border-green-400/30 rounded-lg hover:bg-green-500/30 transition-colors"
                  title="Download trực tiếp"
                >
                  ⬇️ Tải
                </button>
                <button
                  @click="downloadPath = file.fullPath || file.name"
                  class="px-3 py-1 text-sm bg-orange-500/20 text-orange-400 border border-orange-400/30 rounded-lg hover:bg-orange-500/30 transition-colors"
                  title="Chọn để download"
                >
                  📥 Chọn
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Quick Demo Section -->
      <div class="bg-gradient-to-r from-blue-500/20 to-purple-500/20 border border-white/10 rounded-2xl p-8 backdrop-blur-sm">
        <div class="text-center">
          <h3 class="text-xl font-bold text-white mb-4">🚀 Demo nhanh</h3>
          <p class="text-gray-300 mb-6">Thử nghiệm quy trình hoàn chỉnh PGP + SFTP</p>
          
          <div class="grid grid-cols-1 md:grid-cols-3 gap-4 text-sm">
            <div class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-4">
              <div class="text-2xl mb-2">1️⃣</div>
              <div class="font-medium text-white">Mã hóa file ở tab PGP</div>
            </div>
            <div class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-4">
              <div class="text-2xl mb-2">2️⃣</div>
              <div class="font-medium text-white">Upload file đã mã hóa</div>
            </div>
            <div class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-4">
              <div class="text-2xl mb-2">3️⃣</div>
              <div class="font-medium text-white">Download và giải mã</div>
            </div>
          </div>
          
          <router-link to="/pgp" class="inline-flex items-center mt-6 px-6 py-3 bg-gradient-to-r from-blue-500 to-blue-600 text-white font-medium rounded-xl hover:from-blue-400 hover:to-blue-500 transform hover:scale-105 transition-all duration-300">
            🔐 Đi tới PGP
          </router-link>
        </div>
      </div>
    </div>

    <!-- Messages -->
    <div v-if="message" class="fixed bottom-4 right-4 max-w-md z-50">
      <div class="rounded-xl p-4 shadow-lg backdrop-blur-sm" :class="messageType === 'error' ? 'bg-red-500/20 text-red-300 border border-red-400/50' : 'bg-green-500/20 text-green-300 border border-green-400/50'">
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
      uploadPath: '/upload/',
      downloadPath: '',
      connection: {
        host: 'sftp',
        port: 22,
        user: 'demo',
        password: 'pass',
        privateKeyPath: ''
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
        
        // Check both HTTP status and API result
        const isSuccess = response.ok && result.success
        
        this.connectionStatus = {
          success: isSuccess,
          message: result.message || (isSuccess ? 'Kết nối SSH thành công!' : result.error || 'Kết nối SSH thất bại')
        }
        
        if (isSuccess) {
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
      formData.append('password', this.connection.password)
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
        const formData = new FormData()
        formData.append('host', `${this.connection.host}:${this.connection.port}`)
        formData.append('username', this.connection.username)
        formData.append('password', this.connection.password)
        formData.append('remotePath', this.currentPath)
        formData.append('fileName', this.downloadPath.split('/').pop() || this.downloadPath)

        const response = await fetch(`${API_BASE}/sftp/download`, {
          method: 'POST',
          body: formData
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

    async directDownloadFile(file) {
      this.loading = true
      
      try {
        const formData = new FormData()
        formData.append('host', `${this.connection.host}:${this.connection.port}`)
        formData.append('username', this.connection.username)
        formData.append('password', this.connection.password)
        formData.append('remotePath', this.currentPath)
        formData.append('fileName', file.name)

        console.log('Download request:', {
          host: `${this.connection.host}:${this.connection.port}`,
          username: this.connection.username,
          remotePath: this.currentPath,
          fileName: file.name,
          fullPath: file.fullPath
        })

        const response = await fetch(`${API_BASE}/sftp/download`, {
          method: 'POST',
          body: formData
        })

        if (response.ok) {
          const blob = await response.blob()
          const url = window.URL.createObjectURL(blob)
          const a = document.createElement('a')
          a.href = url
          a.download = file.name
          a.click()
          window.URL.revokeObjectURL(url)
          this.showMessage(`Downloaded: ${file.name}`)
        } else {
          const error = await response.json()
          this.showMessage(`Download failed: ${error.error || 'Unknown error'}`, 'error')
          console.error('Download error:', error)
        }
      } catch (error) {
        this.showMessage('Lỗi: ' + error.message, 'error')
        console.error('Download exception:', error)
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
      if (!dateString) return 'N/A'
      try {
        return new Date(dateString).toLocaleDateString('vi-VN', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
          hour: '2-digit',
          minute: '2-digit'
        })
      } catch (error) {
        return 'Invalid Date'
      }
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

<style scoped>
@keyframes blob {
  0% {
    transform: translate(0px, 0px) scale(1);
  }
  33% {
    transform: translate(30px, -50px) scale(1.1);
  }
  66% {
    transform: translate(-20px, 20px) scale(0.9);
  }
  100% {
    transform: translate(0px, 0px) scale(1);
  }
}

.animate-blob {
  animation: blob 7s infinite;
}

.animation-delay-2000 {
  animation-delay: 2s;
}

.animation-delay-4000 {
  animation-delay: 4s;
}

/* Custom scrollbar for webkit browsers */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: transparent;
}

::-webkit-scrollbar-thumb {
  background: rgba(255, 255, 255, 0.2);
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: rgba(255, 255, 255, 0.3);
}
</style>