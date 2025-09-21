<template>
  <div class="min-h-screen bg-slate-900">
    <!-- Glassmorphism Navigation -->
    <nav class="fixed top-0 left-0 right-0 z-50 bg-black/20 backdrop-blur-md border-b border-white/10">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between items-center h-16">
          <!-- Logo and Brand -->
          <div class="flex items-center">
            <div class="flex-shrink-0 flex items-center group">
              <div class="relative">
                <div class="w-10 h-10 bg-gradient-to-r from-cyan-400 to-blue-500 rounded-xl flex items-center justify-center mr-3 group-hover:scale-110 transition-all duration-300">
                  <svg class="w-6 h-6 text-white" fill="currentColor" viewBox="0 0 24 24">
                    <path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4z"/>
                  </svg>
                </div>
                <div class="absolute -inset-1 bg-gradient-to-r from-cyan-400 to-blue-500 rounded-xl blur opacity-0 group-hover:opacity-75 transition duration-300"></div>
              </div>
              <div>
                <h1 class="text-lg font-bold bg-gradient-to-r from-cyan-400 to-blue-500 bg-clip-text text-transparent">
                  VaultLink
                </h1>
                <p class="text-xs text-gray-400 -mt-1">Secure File Transfer</p>
              </div>
            </div>
          </div>

          <!-- Navigation Links -->
          <div class="hidden md:flex items-center space-x-2">
            <router-link
              to="/"
              class="group relative flex items-center px-4 py-2 rounded-xl text-sm font-medium transition-all duration-300"
              :class="$route.path === '/' 
                ? 'text-cyan-400 bg-cyan-400/10 border border-cyan-400/20' 
                : 'text-gray-400 hover:text-white hover:bg-white/10'"
            >
              <span class="mr-2 group-hover:scale-110 transition-transform duration-300">🏠</span>
              Trang chủ
              <div v-if="$route.path === '/'" class="absolute -bottom-2 left-1/2 transform -translate-x-1/2 w-1 h-1 bg-cyan-400 rounded-full"></div>
            </router-link>
            
            <router-link
              to="/pgp"
              class="group relative flex items-center px-4 py-2 rounded-xl text-sm font-medium transition-all duration-300"
              :class="$route.path === '/pgp' 
                ? 'text-blue-400 bg-blue-400/10 border border-blue-400/20' 
                : 'text-gray-400 hover:text-white hover:bg-white/10'"
            >
              <span class="mr-2 group-hover:scale-110 transition-transform duration-300">🔐</span>
              PGP
              <div v-if="$route.path === '/pgp'" class="absolute -bottom-2 left-1/2 transform -translate-x-1/2 w-1 h-1 bg-blue-400 rounded-full"></div>
            </router-link>
            
            <router-link
              to="/ssh-sftp"
              class="group relative flex items-center px-4 py-2 rounded-xl text-sm font-medium transition-all duration-300"
              :class="$route.path === '/ssh-sftp' 
                ? 'text-purple-400 bg-purple-400/10 border border-purple-400/20' 
                : 'text-gray-400 hover:text-white hover:bg-white/10'"
            >
              <span class="mr-2 group-hover:scale-110 transition-transform duration-300">📡</span>
              SFTP
              <div v-if="$route.path === '/ssh-sftp'" class="absolute -bottom-2 left-1/2 transform -translate-x-1/2 w-1 h-1 bg-purple-400 rounded-full"></div>
            </router-link>
          </div>

          <!-- Status & Actions -->
          <div class="flex items-center space-x-3">
            <!-- Connection Status -->
            <div class="hidden sm:flex items-center bg-green-500/20 border border-green-500/30 text-green-400 px-3 py-1 rounded-full text-xs font-medium">
              <span class="w-2 h-2 bg-green-500 rounded-full mr-2 animate-pulse"></span>
              Online
            </div>

            <!-- Quick Actions -->
            <div class="hidden lg:flex items-center space-x-2">
              <button class="p-2 text-gray-400 hover:text-white hover:bg-white/10 rounded-lg transition-all duration-300 group">
                <svg class="w-5 h-5 group-hover:scale-110 transition-transform" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M12 6v6l4 2"/>
                  <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2" fill="none"/>
                </svg>
              </button>
              <button class="p-2 text-gray-400 hover:text-white hover:bg-white/10 rounded-lg transition-all duration-300 group">
                <svg class="w-5 h-5 group-hover:scale-110 transition-transform" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/>
                </svg>
              </button>
            </div>
          </div>
        </div>

        <!-- Mobile Navigation -->
        <div class="md:hidden pb-3">
          <div class="flex space-x-1">
            <router-link
              to="/"
              class="flex-1 flex items-center justify-center px-3 py-2 rounded-lg text-sm font-medium transition-all duration-300"
              :class="$route.path === '/' 
                ? 'text-cyan-400 bg-cyan-400/10' 
                : 'text-gray-400 hover:text-white hover:bg-white/10'"
            >
              <span class="mr-1">🏠</span>
              Home
            </router-link>
            
            <router-link
              to="/pgp"
              class="flex-1 flex items-center justify-center px-3 py-2 rounded-lg text-sm font-medium transition-all duration-300"
              :class="$route.path === '/pgp' 
                ? 'text-blue-400 bg-blue-400/10' 
                : 'text-gray-400 hover:text-white hover:bg-white/10'"
            >
              <span class="mr-1">🔐</span>
              PGP
            </router-link>
            
            <router-link
              to="/ssh-sftp"
              class="flex-1 flex items-center justify-center px-3 py-2 rounded-lg text-sm font-medium transition-all duration-300"
              :class="$route.path === '/ssh-sftp' 
                ? 'text-purple-400 bg-purple-400/10' 
                : 'text-gray-400 hover:text-white hover:bg-white/10'"
            >
              <span class="mr-1">📡</span>
              SFTP
            </router-link>
          </div>
        </div>
      </div>
    </nav>

    <!-- Main Content -->
    <main class="pt-16">
      <router-view />
    </main>

    <!-- Floating Quick Actions (Optional) -->
    <div class="fixed bottom-6 right-6 flex flex-col space-y-3 z-40">
      <button class="w-12 h-12 bg-gradient-to-r from-cyan-500 to-blue-600 text-white rounded-full shadow-lg hover:shadow-xl transform hover:scale-110 transition-all duration-300 flex items-center justify-center group">
        <svg class="w-6 h-6 group-hover:rotate-90 transition-transform duration-300" fill="currentColor" viewBox="0 0 24 24">
          <path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z"/>
        </svg>
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style scoped>
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
