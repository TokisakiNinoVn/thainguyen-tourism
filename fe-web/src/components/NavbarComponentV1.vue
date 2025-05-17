<template>
  <!-- <header
    class="bg-white h-[80px] w-full max-w-[1200px] mx-auto flex items-center justify-between px-6 sticky top-0 z-50 shadow-sm"
  > -->
  <div
    class="main-header bg-white h-[80px] w-full flex items-center justify-between px-6 sticky top-0 z-50 shadow-sm"
  >
    <header
      class="bg-white h-[80px] w-full max-w-[1200px] mx-auto flex items-center justify-between px-6 sticky top-0 z-50 shadow-sm"
    >
      <!-- Logo Section -->
      <div class="flex items-center gap-4">
        <router-link
          to="/"
          class="flex items-center gap-3 transition-transform hover:scale-105"
        >
          <div
            class="bg-gradient-to-r from-green-500 to-emerald-600 rounded-full w-12 h-12 flex items-center justify-center shadow-md"
          >
            <span class="text-white font-bold text-xl">TN</span>
          </div>
          <h1 class="text-green-700 font-bold text-xl tracking-tight">
            Thái Nguyên Travel
          </h1>
        </router-link>
      </div>

      <!-- Navigation Links -->
      <nav class="flex items-center gap-8">
        <router-link
          to="/places"
          class="flex items-center gap-2 text-gray-700 font-medium text-sm hover:text-green-600 transition-colors"
        >
          <i class="fas fa-map-marker-alt"></i>
          Địa điểm
        </router-link>
        <router-link
          to="/introduce"
          class="flex items-center gap-2 text-gray-700 font-medium text-sm hover:text-green-600 transition-colors"
        >
          <i class="fas fa-info-circle"></i>
          Giới thiệu
        </router-link>
        <router-link
          to="/blogs"
          class="flex items-center gap-2 text-gray-700 font-medium text-sm hover:text-green-600 transition-colors"
        >
          <i class="fas fa-newspaper"></i>
          Blogs
        </router-link>
        <router-link
          to="/create-blog"
          class="flex items-center gap-2 text-gray-700 font-medium text-sm hover:text-green-600 transition-colors"
        >
          <i class="fas fa-pen"></i>
          Tạo bài viết
        </router-link>
        <router-link
          to="/chat"
          class="flex items-center gap-2 text-gray-700 font-medium text-sm hover:text-green-600 transition-colors"
        >
          <i class="fas fa-robot"></i>
          Chatbot
        </router-link>
        <router-link
          v-if="isLogin"
          to="/account"
          class="flex items-center gap-2 text-gray-700 font-medium text-sm hover:text-green-600 transition-colors"
        >
          <i class="fas fa-user-circle"></i>
          Tài khoản
        </router-link>
      </nav>

      <!-- Auth Buttons -->
      <div v-if="!isLogin" class="flex items-center gap-4">
        <router-link to="/login">
          <button
            class="flex items-center gap-2 text-green-600 font-medium text-sm px-5 py-2 rounded-full hover:bg-green-50 transition-colors"
          >
            <i class="fas fa-sign-in-alt"></i>
            Đăng nhập
          </button>
        </router-link>
        <router-link to="/register">
          <button
            class="flex items-center gap-2 bg-gradient-to-r from-green-600 to-emerald-700 text-white font-medium text-sm px-5 py-2 rounded-full hover:from-green-700 hover:to-emerald-800 transition-all shadow-sm"
          >
            <i class="fas fa-user-plus"></i>
            Đăng ký
          </button>
        </router-link>
      </div>
    </header>
  </div>
</template>

<script setup>
const isLogin = JSON.parse(localStorage.getItem("isLoggedIn")) || false;
</script>

<style scoped>
/* Custom CSS for specific overrides */
header {
  font-family: "Inter", sans-serif;
}

/* Remove default router-link styles */
router-link {
  text-decoration: none;
}

/* Smooth transitions for buttons */
button {
  transition: all 0.3s ease;
}
</style>
