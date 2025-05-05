<template>
  <header class="bg-white h-[74px] w-full max-w-[966px] mx-auto flex items-center justify-between px-4">
    <!-- Logo Section -->
    <div class="flex items-center gap-3">
      <router-link to="/" class="flex items-center gap-2">

        <div class="bg-green-500 rounded-full w-10 h-10 flex items-center justify-center">
          <span class="text-white font-bold text-[17px]">TN</span>
        </div>
        <h1 class="text-green-700 font-bold text-[17px]">Thái Nguyên Travel</h1>
      </router-link>
    </div>

    <!-- Navigation Links -->
    <nav class="flex items-center gap-6">
      <router-link to="/places" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">Địa điểm</router-link>
    </nav>
    <nav class="flex items-center gap-6">
      <router-link to="/places" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">Giới thiệu</router-link>
    </nav>
    <nav class="flex items-center gap-6">
      <router-link to="/places" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">Blogs</router-link>
    </nav>
    <nav class="flex items-center gap-6">
      <router-link to="/chat" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">Chatbot</router-link>
    </nav>
    <nav class="flex items-center gap-6">
      <router-link to="/places" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">Liên hệ</router-link>
    </nav>
    <nav v-if="isLogin" class="flex items-center gap-6">
      <router-link to="/places" class="text-gray-700 font-medium text-[13.6px] hover:text-green-600">Tài khoản</router-link>
    </nav>

    <!-- Auth Buttons -->
    <div v-if="!isLogin" class="flex items-center gap-4">
      <router-link to="/login">
        <button class="text-green-600 font-normal text-[13.6px] px-4 py-2 rounded-full hover:bg-green-50">Log in</button>
      </router-link>
      <button class="bg-green-600 text-white font-normal text-[13.6px] px-4 py-2 rounded-full hover:bg-green-700">Sign up</button>
    </div>
  </header>
</template>

<script setup>
  const isLogin = JSON.parse(localStorage.getItem('isLoggedIn')) || false;
</script>

<style scoped>
/* Minimal custom CSS for specific overrides or unsupported Tailwind styles */
header {
  font-family: 'Inter', sans-serif;
}

/* Ensure router-link doesn't add unwanted styles */
router-link {
  text-decoration: none;
}
</style>