<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-900 via-gray-900 to-black">
    <!-- Animated Background -->
    <div class="fixed inset-0 overflow-hidden pointer-events-none">
      <div class="blob blob-1"></div>
      <div class="blob blob-2"></div>
      <div class="blob blob-3"></div>
    </div>

    <!-- Glassmorphism Header -->
    <div class="relative z-10 bg-black/20 backdrop-blur-md border-b border-white/10">
      <div class="max-w-6xl mx-auto px-6 py-16">
        <div class="flex items-center group">
          <div class="relative">
            <div class="bg-gradient-to-r from-blue-500 to-purple-600 p-4 rounded-3xl mr-6 group-hover:scale-110 transition-all duration-500">
              <svg class="w-12 h-12 text-white" fill="currentColor" viewBox="0 0 24 24">
                <path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4z"/>
              </svg>
            </div>
            <div class="absolute -inset-2 bg-gradient-to-r from-blue-500 to-purple-600 rounded-3xl blur opacity-0 group-hover:opacity-60 transition duration-500"></div>
          </div>
          <div>
            <h1 class="text-5xl font-bold bg-gradient-to-r from-blue-400 via-purple-400 to-cyan-400 bg-clip-text text-transparent mb-2">
              Quản lý PGP
            </h1>
            <p class="text-gray-300 text-lg">Tạo khóa, mã hóa và ký số file một cách an toàn với công nghệ tiên tiến</p>
          </div>
        </div>
      </div>
    </div>

    <div class="relative z-10 max-w-6xl mx-auto px-6 py-12">
      <!-- Modern Glass Tabs -->
      <div class="bg-white/5 backdrop-blur-xl rounded-3xl border border-white/10 p-2 mb-12 shadow-2xl">
        <nav class="flex space-x-2">
          <button
            v-for="tab in tabs"
            :key="tab.id"
            @click="activeTab = tab.id"
            :class="[
              activeTab === tab.id
                ? 'bg-gradient-to-r from-blue-500 to-purple-600 text-white shadow-lg shadow-blue-500/25'
                : 'text-gray-300 hover:text-white hover:bg-white/10',
              'flex-1 py-4 px-6 text-base font-semibold rounded-2xl transition-all duration-300 flex items-center justify-center group transform hover:scale-105'
            ]"
          >
            <span class="mr-3 text-xl group-hover:scale-110 transition-transform duration-300">{{ tab.icon }}</span>
            {{ tab.name }}
            <div v-if="activeTab === tab.id" class="absolute -bottom-1 left-1/2 transform -translate-x-1/2 w-2 h-2 bg-white rounded-full"></div>
          </button>
        </nav>
      </div>

      <!-- Tab Content -->
      <div class="space-y-12">
        <!-- Keyring Tab -->
        <div v-if="activeTab === 'keyring'">
          <!-- Generate Key Section -->
          <div class="glass-card p-10">
            <div class="flex items-center mb-8 group">
              <div class="relative">
                <div class="bg-gradient-to-r from-green-400 to-emerald-500 p-4 rounded-2xl mr-6 group-hover:scale-110 transition-all duration-300">
                  <svg class="w-8 h-8 text-white" fill="currentColor" viewBox="0 0 24 24">
                    <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/>
                  </svg>
                </div>
                <div class="absolute -inset-1 bg-gradient-to-r from-green-400 to-emerald-500 rounded-2xl blur opacity-0 group-hover:opacity-60 transition duration-300"></div>
              </div>
              <div>
                <h3 class="text-2xl font-bold bg-gradient-to-r from-green-400 to-emerald-500 bg-clip-text text-transparent">Tạo cặp khóa mới</h3>
                <p class="text-gray-400 mt-1">Sinh khóa RSA-4096 bit để mã hóa và ký số</p>
              </div>
            </div>
            
            <div class="grid grid-cols-1 md:grid-cols-3 gap-8 mb-8">
              <div class="space-y-2">
                <label class="block text-sm font-semibold text-gray-300">Họ tên</label>
                <input
                  v-model="generateForm.name"
                  type="text"
                  placeholder="Nguyễn Văn A"
                  class="glass-input"
                />
              </div>
              <div class="space-y-2">
                <label class="block text-sm font-semibold text-gray-300">Email</label>
                <input
                  v-model="generateForm.email"
                  type="email"
                  placeholder="email@example.com"
                  class="glass-input"
                />
              </div>
              <div class="space-y-2">
                <label class="block text-sm font-semibold text-gray-300">Mật khẩu</label>
                <input
                  v-model="generateForm.passphrase"
                  type="password"
                  placeholder="Mật khẩu bảo vệ khóa riêng"
                  class="glass-input"
                />
              </div>
            </div>
            
            <button
              @click="generateKey"
              :disabled="loading"
              class="glass-button bg-gradient-to-r from-green-500 to-emerald-600 hover:from-green-600 hover:to-emerald-700"
            >
              <svg v-if="loading" class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              {{ loading ? 'Đang tạo khóa...' : '🔑 Tạo cặp khóa' }}
            </button>
          </div>

          <!-- Import Keys Section -->
          <div class="glass-card p-10">
            <div class="flex items-center mb-8 group">
              <div class="relative">
                <div class="bg-gradient-to-r from-blue-400 to-cyan-500 p-4 rounded-2xl mr-6 group-hover:scale-110 transition-all duration-300">
                  <svg class="w-8 h-8 text-white" fill="currentColor" viewBox="0 0 24 24">
                    <path d="M14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2M18,20H6V4H13V9H18V20Z"/>
                  </svg>
                </div>
                <div class="absolute -inset-1 bg-gradient-to-r from-blue-400 to-cyan-500 rounded-2xl blur opacity-0 group-hover:opacity-60 transition duration-300"></div>
              </div>
              <div>
                <h3 class="text-2xl font-bold bg-gradient-to-r from-blue-400 to-cyan-500 bg-clip-text text-transparent">Import khóa</h3>
                <p class="text-gray-400 mt-1">Nhập khóa công khai và riêng từ file .asc</p>
              </div>
            </div>
            
            <div class="space-y-8">
              <div>
                <label class="block text-sm font-semibold text-gray-300 mb-3">Khóa công khai (.asc)</label>
                <div class="glass-dropzone group">
                  <input
                    ref="publicKeyFile"
                    type="file"
                    accept=".asc,.txt"
                    @change="importPublicKey"
                    class="hidden"
                  />
                  <button @click="$refs.publicKeyFile.click()" class="text-blue-400 hover:text-blue-300 font-medium group-hover:scale-105 transition-all duration-300">
                    📁 Chọn file khóa công khai
                  </button>
                </div>
              </div>
              
              <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
                <div>
                  <label class="block text-sm font-semibold text-gray-300 mb-3">Khóa riêng (.asc)</label>
                  <div class="glass-dropzone group">
                    <input
                      ref="privateKeyFile"
                      type="file"
                      accept=".asc,.txt"
                      class="hidden"
                    />
                    <button @click="$refs.privateKeyFile.click()" class="text-purple-400 hover:text-purple-300 font-medium group-hover:scale-105 transition-all duration-300">
                      🔐 Chọn file khóa riêng
                    </button>
                  </div>
                </div>
                <div>
                  <label class="block text-sm font-semibold text-gray-300 mb-3">Mật khẩu khóa riêng</label>
                  <input
                    v-model="importPassphrase"
                    type="password"
                    placeholder="Nhập mật khẩu để giải mã khóa riêng"
                    class="glass-input"
                  />
                </div>
              </div>
              
              <button
                @click="importPrivateKey"
                :disabled="loading"
                class="glass-button bg-gradient-to-r from-blue-500 to-purple-600 hover:from-blue-600 hover:to-purple-700"
              >
                {{ loading ? 'Đang import...' : '📥 Import khóa riêng' }}
              </button>
            </div>
          </div>

          <!-- Key List -->
          <div class="glass-card p-10">
            <div class="flex items-center justify-between mb-8">
              <div>
                <h3 class="text-2xl font-bold bg-gradient-to-r from-cyan-400 to-blue-500 bg-clip-text text-transparent">Danh sách khóa</h3>
                <p class="text-gray-400 mt-1">Các khóa PGP hiện có trong hệ thống</p>
              </div>
              <button @click="loadKeys" class="glass-button-small bg-gradient-to-r from-cyan-500 to-blue-600 hover:from-cyan-600 hover:to-blue-700">
                🔄 Tải lại
              </button>
            </div>
            
            <div v-if="keys.length === 0" class="text-center py-16 text-gray-500">
              <div class="text-6xl mb-4">🔑</div>
              <p class="text-lg">Chưa có khóa nào. Hãy tạo hoặc import khóa mới.</p>
            </div>
            
            <div v-else class="space-y-4">
              <div v-for="key in keys" :key="key.id" class="glass-key-card group">
                <div class="flex items-center justify-between">
                  <div class="flex-1">
                    <div class="font-semibold text-white text-lg">{{ key.uid }}</div>
                    <div class="text-gray-300 mt-1">Key ID: {{ key.id }}</div>
                    <div class="text-gray-400 text-sm mt-1">RSA-4096 bits</div>
                    <div class="text-gray-500 text-xs mt-1">{{ formatDate(key.creationTime) }}</div>
                  </div>
                  <div class="flex space-x-3">
                    <span v-if="key.canEncrypt" class="px-3 py-1 bg-green-500/20 border border-green-500/30 text-green-400 text-sm rounded-full font-medium">🔐 Mã hóa</span>
                    <span v-if="key.canSign" class="px-3 py-1 bg-blue-500/20 border border-blue-500/30 text-blue-400 text-sm rounded-full font-medium">✍️ Ký số</span>
                    <span v-if="key.isSecret" class="px-3 py-1 bg-purple-500/20 border border-purple-500/30 text-purple-400 text-sm rounded-full font-medium">🔑 Private</span>
                  </div>
                </div>
                
                <div class="mt-4 pt-4 border-t border-white/5">
                  <div class="flex items-center justify-between">
                    <div class="text-xs text-gray-500 font-mono">{{ key.fingerprint }}</div>
                    <div class="flex space-x-2">
                      <button 
                        @click="selectKeyForEncryption(key.id)"
                        :class="selectedEncryptKey === key.id ? 'bg-blue-600' : 'glass-button-small bg-blue-500/20 hover:bg-blue-500/30'"
                        class="text-blue-400 hover:text-blue-300 px-3 py-1 rounded-lg text-sm transition-all duration-200"
                      >
                        {{ selectedEncryptKey === key.id ? '✓ Đã chọn' : 'Chọn để mã hóa' }}
                      </button>
                      <button class="glass-button-small text-red-400 hover:text-red-300 px-3 py-1 rounded-lg text-sm">
                        🗑️ Xóa
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Encrypt Tab -->
        <div v-if="activeTab === 'encrypt'" class="glass-card p-10">
          <div class="flex items-center mb-8 group">
            <div class="relative">
              <div class="bg-gradient-to-r from-purple-500 to-pink-600 p-4 rounded-2xl mr-6 group-hover:scale-110 transition-all duration-300">
                <svg class="w-8 h-8 text-white" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M12,17A2,2 0 0,0 14,15C14,13.89 13.1,13 12,13A2,2 0 0,0 10,15A2,2 0 0,0 12,17M18,8A2,2 0 0,1 20,10V20A2,2 0 0,1 18,22H6A2,2 0 0,1 4,20V10C4,8.89 4.9,8 6,8H7V6A5,5 0 0,1 12,1A5,5 0 0,1 17,6V8H18M12,3A3,3 0 0,0 9,6V8H15V6A3,3 0 0,0 12,3Z"/>
                </svg>
              </div>
              <div class="absolute -inset-1 bg-gradient-to-r from-purple-500 to-pink-600 rounded-2xl blur opacity-0 group-hover:opacity-60 transition duration-300"></div>
            </div>
            <div>
              <h3 class="text-2xl font-bold bg-gradient-to-r from-purple-400 to-pink-500 bg-clip-text text-transparent">Mã hóa & Ký số</h3>
              <p class="text-gray-400 mt-1">Mã hóa file và tùy chọn ký số với công nghệ bảo mật cao</p>
            </div>
          </div>
          
          <div class="space-y-8">
            <div>
              <label class="block text-sm font-semibold text-gray-300 mb-3">Chọn file cần mã hóa</label>
              <div class="glass-dropzone group">
                <input
                  ref="encryptFileInput"
                  type="file"
                  @change="handleEncryptFileSelect"
                  class="hidden"
                />
                <div class="text-center">
                  <div class="text-4xl mb-3 group-hover:scale-110 transition-transform duration-300">📄</div>
                  <button @click="$refs.encryptFileInput.click()" class="text-purple-400 hover:text-purple-300 font-medium text-lg">
                    {{ selectedFile ? selectedFile.name : 'Chọn file để mã hóa' }}
                  </button>
                  <p class="text-gray-500 text-sm mt-2">Kéo thả file hoặc click để chọn</p>
                </div>
              </div>
            </div>
            
            <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
              <div>
                <label class="block text-sm font-semibold text-gray-300 mb-3">Khóa công khai người nhận</label>
                <select
                  v-model="encryptForm.recipientKeyId"
                  class="glass-input"
                >
                  <option value="">Chọn khóa công khai</option>
                  <option v-for="key in publicKeys" :key="key.id" :value="key.id">
                    {{ key.uid }} ({{ key.id }})
                  </option>
                </select>
              </div>
              
              <div>
                <label class="block text-sm font-semibold text-gray-300 mb-3">Khóa riêng để ký (tùy chọn)</label>
                <select
                  v-model="encryptForm.signKeyId"
                  class="glass-input"
                >
                  <option value="">Không ký số</option>
                  <option v-for="key in privateKeys" :key="key.id" :value="key.id">
                    {{ key.uid }} ({{ key.id }})
                  </option>
                </select>
              </div>
            </div>
            
            <div v-if="encryptForm.signKeyId" class="grid grid-cols-1 md:grid-cols-2 gap-8">
              <div>
                <label class="block text-sm font-semibold text-gray-300 mb-3">Mật khẩu khóa riêng</label>
                <input
                  v-model="encryptForm.passphrase"
                  type="password"
                  placeholder="Mật khẩu để mở khóa riêng"
                  class="glass-input"
                />
              </div>
              
              <div class="flex items-end">
                <label class="flex items-center glass-checkbox-container">
                  <input
                    v-model="encryptForm.armor"
                    type="checkbox"
                    class="sr-only"
                  />
                  <div class="glass-checkbox"></div>
                  <span class="ml-3 text-gray-300 font-medium">ASCII Armor (.asc)</span>
                </label>
              </div>
            </div>
            
            <button
              @click="encryptFile"
              :disabled="loading || !selectedFile || !encryptForm.recipientKeyId"
              class="glass-button bg-gradient-to-r from-purple-500 to-pink-600 hover:from-purple-600 hover:to-pink-700"
            >
              {{ loading ? 'Đang mã hóa...' : '🔐 Mã hóa & Download' }}
            </button>
          </div>
        </div>

        <!-- Decrypt Tab -->
        <div v-if="activeTab === 'decrypt'" class="glass-card p-10">
          <div class="flex items-center mb-8 group">
            <div class="relative">
              <div class="bg-gradient-to-r from-orange-500 to-red-600 p-4 rounded-2xl mr-6 group-hover:scale-110 transition-all duration-300">
                <svg class="w-8 h-8 text-white" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M12,17A2,2 0 0,0 14,15C14,13.89 13.1,13 12,13A2,2 0 0,0 10,15A2,2 0 0,0 12,17M18,8A2,2 0 0,1 20,10V20A2,2 0 0,1 18,22H6A2,2 0 0,1 4,20V10C4,8.89 4.9,8 6,8H7V6A5,5 0 0,1 12,1A5,5 0 0,1 17,6V8H18M12,3A3,3 0 0,0 9,6V8H15V6A3,3 0 0,0 12,3Z"/>
                </svg>
              </div>
              <div class="absolute -inset-1 bg-gradient-to-r from-orange-500 to-red-600 rounded-2xl blur opacity-0 group-hover:opacity-60 transition duration-300"></div>
            </div>
            <div>
              <h3 class="text-2xl font-bold bg-gradient-to-r from-orange-400 to-red-500 bg-clip-text text-transparent">Giải mã & Xác thực</h3>
              <p class="text-gray-400 mt-1">Giải mã file và xác thực chữ ký số một cách an toàn</p>
            </div>
          </div>
          
          <div class="space-y-8">
            <div>
              <label class="block text-sm font-semibold text-gray-300 mb-3">Chọn file cần giải mã</label>
              <div class="glass-dropzone group">
                <input
                  ref="decryptFileInput"
                  type="file"
                  @change="handleDecryptFileSelect"
                  accept=".pgp,.asc"
                  class="hidden"
                />
                <div class="text-center">
                  <div class="text-4xl mb-3 group-hover:scale-110 transition-transform duration-300">🔒</div>
                  <button @click="$refs.decryptFileInput.click()" class="text-orange-400 hover:text-orange-300 font-medium text-lg">
                    {{ selectedDecryptFile ? selectedDecryptFile.name : 'Chọn file .pgp hoặc .asc' }}
                  </button>
                  <p class="text-gray-500 text-sm mt-2">Hỗ trợ file .pgp và .asc</p>
                </div>
              </div>
            </div>
            
            <div>
              <label class="block text-sm font-semibold text-gray-300 mb-3">Mật khẩu khóa riêng</label>
              <input
                v-model="decryptForm.passphrase"
                type="password"
                placeholder="Mật khẩu để giải mã file"
                class="glass-input"
              />
            </div>
            
            <button
              @click="decryptFile"
              :disabled="loading || !selectedDecryptFile || !decryptForm.passphrase"
              class="glass-button bg-gradient-to-r from-orange-500 to-red-600 hover:from-orange-600 hover:to-red-700"
            >
              {{ loading ? 'Đang giải mã...' : '🔓 Giải mã & Download' }}
            </button>
            
            <!-- Decrypt Result -->
            <div v-if="decryptResult" class="glass-result-card">
              <h4 class="font-semibold text-white mb-4 flex items-center">
                <span class="text-2xl mr-3">✨</span>
                Kết quả giải mã
              </h4>
              <div class="space-y-3 text-sm">
                <div class="flex items-center">
                  <strong class="text-gray-300 w-24">Tên file:</strong>
                  <span class="text-white">{{ decryptResult.filename }}</span>
                </div>
                <div v-if="decryptResult.signerKeyId" class="flex items-center">
                  <strong class="text-gray-300 w-24">Chữ ký:</strong>
                  <span :class="decryptResult.signatureValid ? 'text-green-400' : 'text-red-400'" class="flex items-center">
                    {{ decryptResult.signatureValid ? '✅ Hợp lệ' : '❌ Không hợp lệ' }}
                    <span class="ml-2 text-gray-400">({{ decryptResult.signerKeyId }})</span>
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Glassmorphism Messages -->
    <div v-if="message" class="fixed bottom-6 right-6 max-w-md z-50">
      <div class="glass-message" :class="messageType === 'error' ? 'border-red-500/50 bg-red-500/10' : 'border-green-500/50 bg-green-500/10'">
        <div class="flex items-center">
          <span class="mr-3 text-2xl">{{ messageType === 'error' ? '❌' : '✅' }}</span>
          <span :class="messageType === 'error' ? 'text-red-300' : 'text-green-300'" class="font-medium">{{ message }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
const API_BASE = import.meta.env.VITE_API_BASE || 'http://localhost:5080'

export default {
  name: 'Pgp',
  data() {
    return {
      activeTab: 'keyring',
      loading: false,
      message: '',
      messageType: 'success',
      keys: [],
      selectedFile: null,
      selectedDecryptFile: null,
      decryptResult: null,
      importPassphrase: '',
      selectedEncryptKey: null,
      tabs: [
        { id: 'keyring', name: 'Quản lý khóa', icon: '🔑' },
        { id: 'encrypt', name: 'Mã hóa/Ký', icon: '🔐' },
        { id: 'decrypt', name: 'Giải mã/Xác thực', icon: '🔓' }
      ],
      generateForm: {
        name: '',
        email: '',
        passphrase: ''
      },
      encryptForm: {
        recipientKeyId: '',
        signKeyId: '',
        passphrase: '',
        armor: true
      },
      decryptForm: {
        passphrase: ''
      }
    }
  },
  computed: {
    publicKeys() {
      return this.keys.filter(key => key.canEncrypt)
    },
    privateKeys() {
      return this.keys.filter(key => key.isSecret)
    }
  },
  async mounted() {
    await this.loadKeys()
  },
  methods: {
    async loadKeys() {
      try {
        const response = await fetch(`${API_BASE}/pgp/keys`)
        if (response.ok) {
          this.keys = await response.json()
        }
      } catch (error) {
        console.error('Failed to load keys:', error)
      }
    },
    async generateKey() {
      if (!this.generateForm.name || !this.generateForm.email || !this.generateForm.passphrase) {
        this.showMessage('Vui lòng điền đầy đủ thông tin', 'error')
        return
      }

      this.loading = true
      try {
        const response = await fetch(`${API_BASE}/pgp/generate`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(this.generateForm)
        })

        if (response.ok) {
          const result = await response.json()
          this.showMessage(`Tạo cặp khóa thành công! Key ID: ${result.keyId}`)
          this.generateForm = { name: '', email: '', passphrase: '' }
          await this.loadKeys()
        } else {
          const error = await response.json()
          this.showMessage(error.error || 'Tạo khóa thất bại', 'error')
        }
      } catch (error) {
        this.showMessage('Lỗi: ' + error.message, 'error')
      } finally {
        this.loading = false
      }
    },
    async importPublicKey(event) {
      const file = event.target.files[0]
      if (!file) return

      this.loading = true
      const formData = new FormData()
      formData.append('file', file)

      try {
        const response = await fetch(`${API_BASE}/pgp/import-public`, {
          method: 'POST',
          body: formData
        })

        if (response.ok) {
          this.showMessage('Import khóa công khai thành công!')
          this.$refs.publicKeyFile.value = ''
          await this.loadKeys()
        } else {
          const error = await response.json()
          this.showMessage(error.error || 'Import khóa công khai thất bại', 'error')
        }
      } catch (error) {
        this.showMessage('Lỗi: ' + error.message, 'error')
      } finally {
        this.loading = false
      }
    },
    async importPrivateKey() {
      const file = this.$refs.privateKeyFile.files[0]
      if (!file || !this.importPassphrase) {
        this.showMessage('Vui lòng chọn file và nhập mật khẩu', 'error')
        return
      }

      this.loading = true
      const formData = new FormData()
      formData.append('file', file)
      formData.append('passphrase', this.importPassphrase)

      try {
        const response = await fetch(`${API_BASE}/pgp/import-private`, {
          method: 'POST',
          body: formData
        })

        if (response.ok) {
          this.showMessage('Import khóa riêng thành công!')
          this.$refs.privateKeyFile.value = ''
          this.importPassphrase = ''
          await this.loadKeys()
        } else {
          const error = await response.json()
          this.showMessage(error.error || 'Import khóa riêng thất bại', 'error')
        }
      } catch (error) {
        this.showMessage('Lỗi: ' + error.message, 'error')
      } finally {
        this.loading = false
      }
    },
    handleEncryptFileSelect(event) {
      this.selectedFile = event.target.files[0]
    },
    handleDecryptFileSelect(event) {
      this.selectedDecryptFile = event.target.files[0]
    },
    async encryptFile() {
      if (!this.selectedFile || !this.encryptForm.recipientKeyId) return

      this.loading = true
      const formData = new FormData()
      formData.append('file', this.selectedFile)
      formData.append('recipientKeyId', this.encryptForm.recipientKeyId)
      formData.append('armor', this.encryptForm.armor.toString())
      
      if (this.encryptForm.signKeyId) {
        formData.append('signKeyId', this.encryptForm.signKeyId)
        formData.append('passphrase', this.encryptForm.passphrase)
      }

      try {
        const response = await fetch(`${API_BASE}/pgp/encrypt`, {
          method: 'POST',
          body: formData
        })

        if (response.ok) {
          const blob = await response.blob()
          const url = window.URL.createObjectURL(blob)
          const a = document.createElement('a')
          a.href = url
          a.download = this.encryptForm.armor ? `${this.selectedFile.name}.asc` : `${this.selectedFile.name}.pgp`
          a.click()
          window.URL.revokeObjectURL(url)
          this.showMessage('Mã hóa và tải xuống thành công!')
        } else {
          const error = await response.json()
          this.showMessage(error.error || 'Mã hóa file thất bại', 'error')
        }
      } catch (error) {
        this.showMessage('Lỗi: ' + error.message, 'error')
      } finally {
        this.loading = false
      }
    },
    async decryptFile() {
      if (!this.selectedDecryptFile || !this.decryptForm.passphrase) return

      this.loading = true
      const formData = new FormData()
      formData.append('file', this.selectedDecryptFile)
      formData.append('passphrase', this.decryptForm.passphrase)

      try {
        const response = await fetch(`${API_BASE}/pgp/decrypt`, {
          method: 'POST',
          body: formData
        })

        if (response.ok) {
          const blob = await response.blob()
          const url = window.URL.createObjectURL(blob)
          const a = document.createElement('a')
          a.href = url
          a.download = this.selectedDecryptFile.name.replace(/\.(pgp|asc)$/, '')
          a.click()
          window.URL.revokeObjectURL(url)
          this.showMessage('Giải mã và tải xuống thành công!')
        } else {
          const error = await response.json()
          this.showMessage(error.error || 'Giải mã file thất bại', 'error')
        }
      } catch (error) {
        this.showMessage('Lỗi: ' + error.message, 'error')
      } finally {
        this.loading = false
      }
    },
    showMessage(text, type = 'success') {
      this.message = text
      this.messageType = type
      setTimeout(() => {
        this.message = ''
      }, 5000)
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
    selectKeyForEncryption(keyId) {
      this.selectedEncryptKey = keyId
      this.showMessage(`Đã chọn khóa ${keyId} để mã hóa`)
    }
  }
}
</script>

<style scoped>
/* Animated Background Blobs */
.blob {
  position: absolute;
  border-radius: 50%;
  filter: blur(40px);
  mix-blend-mode: multiply;
  animation: blob 7s infinite;
}

.blob-1 {
  top: 0;
  left: 0;
  width: 300px;
  height: 300px;
  background: linear-gradient(45deg, rgba(59, 130, 246, 0.3), rgba(147, 51, 234, 0.3));
  animation-delay: 0s;
}

.blob-2 {
  top: 50%;
  right: 0;
  width: 350px;
  height: 350px;
  background: linear-gradient(45deg, rgba(236, 72, 153, 0.3), rgba(59, 130, 246, 0.3));
  animation-delay: 2s;
}

.blob-3 {
  bottom: 0;
  left: 50%;
  width: 400px;
  height: 400px;
  background: linear-gradient(45deg, rgba(34, 197, 94, 0.3), rgba(168, 85, 247, 0.3));
  animation-delay: 4s;
}

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

/* Glassmorphism Components */
.glass-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 2rem;
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
  transition: all 0.3s ease;
}

.glass-card:hover {
  background: rgba(255, 255, 255, 0.08);
  border-color: rgba(255, 255, 255, 0.2);
  transform: translateY(-2px);
}

.glass-input {
  width: 100%;
  padding: 1rem 1.5rem;
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 1rem;
  color: white;
  font-size: 0.95rem;
  transition: all 0.3s ease;
}

.glass-input:focus {
  outline: none;
  background: rgba(255, 255, 255, 0.1);
  border-color: rgba(59, 130, 246, 0.5);
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.glass-input::placeholder {
  color: rgba(255, 255, 255, 0.4);
}

.glass-button {
  display: inline-flex;
  align-items: center;
  padding: 1rem 2rem;
  font-weight: 600;
  font-size: 1rem;
  color: white;
  border: none;
  border-radius: 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
  backdrop-filter: blur(10px);
  box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.2);
}

.glass-button:hover {
  transform: translateY(-2px) scale(1.02);
  box-shadow: 0 20px 40px -5px rgba(0, 0, 0, 0.3);
}

.glass-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none;
}

.glass-button-small {
  padding: 0.75rem 1.5rem;
  font-size: 0.9rem;
  border-radius: 0.75rem;
  display: inline-flex;
  align-items: center;
  font-weight: 600;
  color: white;
  border: none;
  cursor: pointer;
  transition: all 0.3s ease;
  backdrop-filter: blur(10px);
  box-shadow: 0 8px 20px -5px rgba(0, 0, 0, 0.2);
}

.glass-button-small:hover {
  transform: translateY(-1px) scale(1.05);
  box-shadow: 0 15px 30px -5px rgba(0, 0, 0, 0.3);
}

.glass-dropzone {
  border: 2px dashed rgba(255, 255, 255, 0.2);
  border-radius: 1.5rem;
  padding: 3rem 2rem;
  background: rgba(255, 255, 255, 0.03);
  backdrop-filter: blur(10px);
  transition: all 0.3s ease;
}

.glass-dropzone:hover {
  border-color: rgba(59, 130, 246, 0.5);
  background: rgba(255, 255, 255, 0.08);
  transform: scale(1.02);
}

.glass-key-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(15px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 1.5rem;
  padding: 1.5rem;
  transition: all 0.3s ease;
}

.glass-key-card:hover {
  background: rgba(255, 255, 255, 0.1);
  border-color: rgba(255, 255, 255, 0.2);
  transform: translateY(-2px);
  box-shadow: 0 15px 35px -5px rgba(0, 0, 0, 0.2);
}

.glass-result-card {
  background: rgba(34, 197, 94, 0.05);
  backdrop-filter: blur(15px);
  border: 1px solid rgba(34, 197, 94, 0.2);
  border-radius: 1.5rem;
  padding: 1.5rem;
  margin-top: 2rem;
}

.glass-message {
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(20px);
  border: 1px solid;
  border-radius: 1rem;
  padding: 1rem 1.5rem;
  box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.2);
  animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
  from {
    transform: translateX(100%);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}

.glass-checkbox-container {
  position: relative;
  display: flex;
  align-items: center;
  cursor: pointer;
}

.glass-checkbox {
  width: 1.25rem;
  height: 1.25rem;
  background: rgba(255, 255, 255, 0.05);
  border: 2px solid rgba(255, 255, 255, 0.2);
  border-radius: 0.375rem;
  transition: all 0.3s ease;
  position: relative;
}

.glass-checkbox-container input:checked + .glass-checkbox {
  background: linear-gradient(45deg, #8b5cf6, #a855f7);
  border-color: #8b5cf6;
}

.glass-checkbox-container input:checked + .glass-checkbox::after {
  content: '✓';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  color: white;
  font-size: 0.75rem;
  font-weight: bold;
}

/* Custom scrollbar */
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