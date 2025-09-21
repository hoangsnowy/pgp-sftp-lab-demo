<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900">
    <!-- Animated Background -->
    <div class="absolute inset-0 overflow-hidden">
      <div class="absolute -inset-10 opacity-50">
        <div class="absolute top-0 -left-4 w-72 h-72 bg-purple-300 rounded-full mix-blend-multiply filter blur-xl opacity-70 animate-blob"></div>
        <div class="absolute top-0 -right-4 w-72 h-72 bg-yellow-300 rounded-full mix-blend-multiply filter blur-xl opacity-70 animate-blob animation-delay-2000"></div>
        <div class="absolute -bottom-8 left-20 w-72 h-72 bg-pink-300 rounded-full mix-blend-multiply filter blur-xl opacity-70 animate-blob animation-delay-4000"></div>
      </div>
    </div>

    <!-- Hero Section -->
    <div class="relative">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 pt-20 pb-16">
        <div class="text-center">
          <!-- Logo Animation -->
          <div class="flex justify-center mb-8">
            <div class="relative">
              <div class="w-20 h-20 bg-gradient-to-r from-cyan-400 to-blue-500 rounded-3xl flex items-center justify-center shadow-2xl transform hover:scale-110 transition-all duration-500">
                <svg class="w-10 h-10 text-white" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4z"/>
                </svg>
              </div>
              <div class="absolute -inset-1 bg-gradient-to-r from-cyan-400 to-blue-500 rounded-3xl blur opacity-75 group-hover:opacity-100 transition duration-1000 group-hover:duration-200 animate-tilt"></div>
            </div>
          </div>

          <!-- Title with Gradient -->
          <h1 class="text-6xl md:text-7xl font-black mb-6">
            <span class="bg-gradient-to-r from-cyan-400 via-blue-500 to-purple-600 bg-clip-text text-transparent">
              VaultLink
            </span>
          </h1>
          
          <p class="text-xl md:text-2xl text-gray-300 mb-8 max-w-3xl mx-auto leading-relaxed">
            🔐 Hệ thống mã hóa và truyền file 
            <span class="text-cyan-400 font-semibold">siêu bảo mật</span> 
            với công nghệ PGP + SFTP
          </p>

          <!-- Feature Badges -->
          <div class="flex flex-wrap justify-center gap-4 mb-12">
            <div class="bg-white/10 backdrop-blur-sm border border-white/20 px-6 py-3 rounded-full text-white font-medium hover:bg-white/20 transition-all duration-300">
              �️ RSA-4096 bit
            </div>
            <div class="bg-white/10 backdrop-blur-sm border border-white/20 px-6 py-3 rounded-full text-white font-medium hover:bg-white/20 transition-all duration-300">
              🔒 AES-256 encryption
            </div>
            <div class="bg-white/10 backdrop-blur-sm border border-white/20 px-6 py-3 rounded-full text-white font-medium hover:bg-white/20 transition-all duration-300">
              📡 SFTP transfer
            </div>
            <div class="bg-white/10 backdrop-blur-sm border border-white/20 px-6 py-3 rounded-full text-white font-medium hover:bg-white/20 transition-all duration-300">
              ✍️ Digital signature
            </div>
          </div>

          <!-- CTA Buttons -->
          <div class="flex flex-col sm:flex-row gap-4 justify-center">
            <router-link to="/pgp" 
              class="group relative inline-flex items-center justify-center px-8 py-4 text-lg font-medium text-white bg-gradient-to-r from-cyan-500 to-blue-600 rounded-2xl hover:from-cyan-400 hover:to-blue-500 transform hover:scale-105 transition-all duration-300 shadow-2xl">
              <span class="absolute -inset-0.5 bg-gradient-to-r from-cyan-500 to-blue-600 rounded-2xl blur opacity-75 group-hover:opacity-100 transition duration-300"></span>
              <span class="relative flex items-center">
                🔐 Bắt đầu mã hóa
                <svg class="w-5 h-5 ml-2 group-hover:translate-x-1 transition-transform" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z"/>
                </svg>
              </span>
            </router-link>
            
            <router-link to="/ssh-sftp" 
              class="group relative inline-flex items-center justify-center px-8 py-4 text-lg font-medium text-white bg-white/10 backdrop-blur-sm border border-white/20 rounded-2xl hover:bg-white/20 transform hover:scale-105 transition-all duration-300">
              📡 Quản lý SFTP
              <svg class="w-5 h-5 ml-2 group-hover:translate-x-1 transition-transform" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z"/>
              </svg>
            </router-link>
          </div>
        </div>
      </div>
    </div>

    <!-- Process Flow Section -->
    <div class="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-20">
      <div class="text-center mb-16">
        <h2 class="text-4xl md:text-5xl font-bold text-white mb-6">
          Quy trình bảo mật 
          <span class="bg-gradient-to-r from-cyan-400 to-purple-400 bg-clip-text text-transparent">4 bước</span>
        </h2>
        <p class="text-xl text-gray-300 max-w-3xl mx-auto">
          Từ file gốc đến file được bảo vệ hoàn toàn, chỉ trong vài click chuột
        </p>
      </div>
      
      <!-- Interactive Process Steps -->
      <div class="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-6 md:gap-8">
        <!-- Step 1 -->
        <div class="group relative min-h-[280px]">
          <div class="h-full bg-white/5 backdrop-blur-sm border border-white/10 rounded-3xl p-6 md:p-8 hover:bg-white/10 hover:border-cyan-400/50 transition-all duration-500 transform hover:-translate-y-2">
            <div class="absolute -top-4 -left-4 w-12 h-12 bg-gradient-to-r from-green-400 to-cyan-400 rounded-2xl flex items-center justify-center text-white font-bold text-xl shadow-lg">
              1
            </div>
            <div class="w-16 h-16 bg-gradient-to-r from-green-400 to-cyan-400 rounded-2xl flex items-center justify-center mx-auto mb-6 group-hover:scale-110 transition-transform duration-300">
              <span class="text-3xl">📄</span>
            </div>
            <h3 class="text-xl font-bold text-white mb-3">Upload File</h3>
            <p class="text-gray-400 leading-relaxed">Chọn file cần bảo vệ từ máy tính của bạn</p>
          </div>
        </div>

        <!-- Step 2 -->
        <div class="group relative min-h-[280px]">
          <div class="h-full bg-white/5 backdrop-blur-sm border border-white/10 rounded-3xl p-6 md:p-8 hover:bg-white/10 hover:border-blue-400/50 transition-all duration-500 transform hover:-translate-y-2">
            <div class="absolute -top-4 -left-4 w-12 h-12 bg-gradient-to-r from-blue-400 to-purple-400 rounded-2xl flex items-center justify-center text-white font-bold text-xl shadow-lg">
              2
            </div>
            <div class="w-16 h-16 bg-gradient-to-r from-blue-400 to-purple-400 rounded-2xl flex items-center justify-center mx-auto mb-6 group-hover:scale-110 transition-transform duration-300">
              <span class="text-3xl">🔐</span>
            </div>
            <h3 class="text-xl font-bold text-white mb-3">Mã hóa PGP</h3>
            <p class="text-gray-400 leading-relaxed">Sử dụng RSA-4096 + AES-256 để bảo vệ</p>
          </div>
        </div>

        <!-- Step 3 -->
        <div class="group relative min-h-[280px]">
          <div class="h-full bg-white/5 backdrop-blur-sm border border-white/10 rounded-3xl p-6 md:p-8 hover:bg-white/10 hover:border-purple-400/50 transition-all duration-500 transform hover:-translate-y-2">
            <div class="absolute -top-4 -left-4 w-12 h-12 bg-gradient-to-r from-purple-400 to-pink-400 rounded-2xl flex items-center justify-center text-white font-bold text-xl shadow-lg">
              3
            </div>
            <div class="w-16 h-16 bg-gradient-to-r from-purple-400 to-pink-400 rounded-2xl flex items-center justify-center mx-auto mb-6 group-hover:scale-110 transition-transform duration-300">
              <span class="text-3xl">📡</span>
            </div>
            <h3 class="text-xl font-bold text-white mb-3">SFTP Transfer</h3>
            <p class="text-gray-400 leading-relaxed">Truyền file qua kênh SSH an toàn</p>
          </div>
        </div>

        <!-- Step 4 -->
        <div class="group relative min-h-[280px]">
          <div class="h-full bg-white/5 backdrop-blur-sm border border-white/10 rounded-3xl p-6 md:p-8 hover:bg-white/10 hover:border-pink-400/50 transition-all duration-500 transform hover:-translate-y-2">
            <div class="absolute -top-4 -left-4 w-12 h-12 bg-gradient-to-r from-pink-400 to-red-400 rounded-2xl flex items-center justify-center text-white font-bold text-xl shadow-lg">
              4
            </div>
            <div class="w-16 h-16 bg-gradient-to-r from-pink-400 to-red-400 rounded-2xl flex items-center justify-center mx-auto mb-6 group-hover:scale-110 transition-transform duration-300">
              <span class="text-3xl">🔓</span>
            </div>
            <h3 class="text-xl font-bold text-white mb-3">Giải mã</h3>
            <p class="text-gray-400 leading-relaxed">Khôi phục file gốc an toàn</p>
          </div>
        </div>
      </div>

      <!-- Animated Arrow Flow -->
      <div class="hidden lg:block absolute top-1/2 left-0 right-0 pointer-events-none">
        <div class="relative h-1">
          <div class="absolute inset-0 flex items-center justify-between px-8">
            <div class="flex-1 h-px bg-gradient-to-r from-transparent via-cyan-400 to-transparent mx-4 opacity-50"></div>
            <div class="flex-1 h-px bg-gradient-to-r from-transparent via-blue-400 to-transparent mx-4 opacity-50"></div>
            <div class="flex-1 h-px bg-gradient-to-r from-transparent via-purple-400 to-transparent mx-4 opacity-50"></div>
          </div>
        </div>
      </div>
    </div>

    <!-- Feature Showcase -->
    <div class="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-20">
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-16 items-center">
        <!-- PGP Feature -->
        <div class="group">
          <div class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-3xl p-8 hover:bg-white/10 transition-all duration-500">
            <div class="flex items-center mb-6">
              <div class="w-16 h-16 bg-gradient-to-r from-cyan-400 to-blue-500 rounded-2xl flex items-center justify-center mr-6">
                <svg class="w-8 h-8 text-white" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4z"/>
                </svg>
              </div>
              <div>
                <h3 class="text-2xl font-bold text-white mb-2">PGP Cryptography</h3>
                <p class="text-cyan-400 font-medium">Mã hóa cấp quân đội</p>
              </div>
            </div>
            
            <div class="space-y-4">
              <div class="flex items-center text-gray-300">
                <div class="w-2 h-2 bg-cyan-400 rounded-full mr-4"></div>
                <span>Tạo cặp khóa RSA-4096 bit siêu mạnh</span>
              </div>
              <div class="flex items-center text-gray-300">
                <div class="w-2 h-2 bg-blue-400 rounded-full mr-4"></div>
                <span>Mã hóa AES-256 không thể phá vỡ</span>
              </div>
              <div class="flex items-center text-gray-300">
                <div class="w-2 h-2 bg-purple-400 rounded-full mr-4"></div>
                <span>Chữ ký số đảm bảo tính toàn vẹn</span>
              </div>
              <div class="flex items-center text-gray-300">
                <div class="w-2 h-2 bg-pink-400 rounded-full mr-4"></div>
                <span>Import/Export khóa dễ dàng</span>
              </div>
            </div>
            
            <router-link to="/pgp" 
              class="inline-flex items-center mt-8 px-6 py-3 bg-gradient-to-r from-cyan-500 to-blue-600 text-white font-medium rounded-xl hover:from-cyan-400 hover:to-blue-500 transform hover:scale-105 transition-all duration-300">
              <span>Khám phá PGP</span>
              <svg class="w-5 h-5 ml-2" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z"/>
              </svg>
            </router-link>
          </div>
        </div>

        <!-- SFTP Feature -->
        <div class="group">
          <div class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-3xl p-8 hover:bg-white/10 transition-all duration-500">
            <div class="flex items-center mb-6">
              <div class="w-16 h-16 bg-gradient-to-r from-purple-400 to-pink-500 rounded-2xl flex items-center justify-center mr-6">
                <svg class="w-8 h-8 text-white" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M19.35 10.04A7.49 7.49 0 0 0 12 4C9.11 4 6.6 5.64 5.35 8.04A5.994 5.994 0 0 0 0 14c0 3.31 2.69 6 6 6h13c2.76 0 5-2.24 5-5 0-2.64-2.05-4.78-4.65-4.96z"/>
                </svg>
              </div>
              <div>
                <h3 class="text-2xl font-bold text-white mb-2">SSH/SFTP Transfer</h3>
                <p class="text-purple-400 font-medium">Truyền file siêu an toàn</p>
              </div>
            </div>
            
            <div class="space-y-4">
              <div class="flex items-center text-gray-300">
                <div class="w-2 h-2 bg-purple-400 rounded-full mr-4"></div>
                <span>Kết nối SSH với Ed25519 keys</span>
              </div>
              <div class="flex items-center text-gray-300">
                <div class="w-2 h-2 bg-pink-400 rounded-full mr-4"></div>
                <span>Upload/Download tốc độ cao</span>
              </div>
              <div class="flex items-center text-gray-300">
                <div class="w-2 h-2 bg-red-400 rounded-full mr-4"></div>
                <span>Quản lý file remote trực quan</span>
              </div>
              <div class="flex items-center text-gray-300">
                <div class="w-2 h-2 bg-orange-400 rounded-full mr-4"></div>
                <span>Giao diện kéo thả hiện đại</span>
              </div>
            </div>
            
            <router-link to="/ssh-sftp" 
              class="inline-flex items-center mt-8 px-6 py-3 bg-gradient-to-r from-purple-500 to-pink-600 text-white font-medium rounded-xl hover:from-purple-400 hover:to-pink-500 transform hover:scale-105 transition-all duration-300">
              <span>Khám phá SFTP</span>
              <svg class="w-5 h-5 ml-2" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z"/>
              </svg>
            </router-link>
          </div>
        </div>
      </div>
    </div>

    <!-- Stats Section -->
    <div class="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-20">
      <div class="bg-white/5 backdrop-blur-sm border border-white/10 rounded-3xl p-8">
        <div class="grid grid-cols-2 md:grid-cols-4 gap-8">
          <div class="text-center group">
            <div class="text-4xl md:text-5xl font-black bg-gradient-to-r from-cyan-400 to-blue-500 bg-clip-text text-transparent mb-2">
              4096
            </div>
            <div class="text-gray-400 font-medium">RSA Key Size (bits)</div>
          </div>
          <div class="text-center group">
            <div class="text-4xl md:text-5xl font-black bg-gradient-to-r from-blue-400 to-purple-500 bg-clip-text text-transparent mb-2">
              256
            </div>
            <div class="text-gray-400 font-medium">AES Encryption (bits)</div>
          </div>
          <div class="text-center group">
            <div class="text-4xl md:text-5xl font-black bg-gradient-to-r from-purple-400 to-pink-500 bg-clip-text text-transparent mb-2">
              2222
            </div>
            <div class="text-gray-400 font-medium">SFTP Port</div>
          </div>
          <div class="text-center group">
            <div class="text-4xl md:text-5xl font-black bg-gradient-to-r from-pink-400 to-red-500 bg-clip-text text-transparent mb-2">
              ∞
            </div>
            <div class="text-gray-400 font-medium">Security Level</div>
          </div>
        </div>
      </div>
    </div>

    <!-- Footer -->
    <div class="relative border-t border-white/10 bg-black/20">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <div class="text-center">
          <p class="text-gray-400 mb-4">
            VaultLink Demo - Hệ thống mã hóa và truyền file siêu bảo mật
          </p>
          <div class="flex flex-wrap justify-center gap-4 text-sm text-gray-500">
            <span>ASP.NET 8</span>
            <span>•</span>
            <span>Vue 3</span>
            <span>•</span>
            <span>Docker</span>
            <span>•</span>
            <span>BouncyCastle</span>
            <span>•</span>
            <span>SSH.NET</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
// Dashboard logic can be added here if needed
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

@keyframes tilt {
  0%, 50%, 100% {
    transform: rotate(0deg);
  }
  25% {
    transform: rotate(1deg);
  }
  75% {
    transform: rotate(-1deg);
  }
}

.animate-blob {
  animation: blob 7s infinite;
}

.animate-tilt {
  animation: tilt 10s infinite linear;
}

.animation-delay-2000 {
  animation-delay: 2s;
}

.animation-delay-4000 {
  animation-delay: 4s;
}
</style>