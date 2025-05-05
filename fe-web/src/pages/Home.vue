<template>
    <div id="app">
      <!-- Header -->
      <h1>✌️Chào mừng bạn đến với website du lịch  tỉnh Thái Nguyên!</h1>
      <NavbarComponentV1 />

      <!-- Banner page -->
      
      <div class="banner">
        <img src="../assets/images/image-banner.jpg" alt="Banner" style="width: 100%; height: auto;">
      </div>

      <!-- Footer -->
      <FooterComponent />
    </div>
  </template>
  
  <script>
  import { computed } from 'vue';
  import { useRouter } from 'vue-router';
  import NavbarComponentV1 from '@/components/NavbarComponentV1.vue';
  import FooterComponent from '@/components/FooterComponent.vue';
  
  export default {
    name: 'App',
    components: {
      NavbarComponentV1,
      FooterComponent,
    },
    setup() {
      const router = useRouter();
  
      // Kiểm tra người dùng có đăng nhập hay không
      const isAuthenticated = computed(() => !!localStorage.getItem('token'));
  
      return {
        isAuthenticated,
        router,
      };
    },
  };
  </script>
  
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
  display: flex;
  flex-direction: column;
}

header {
  background-color: #f8f9fa;
  padding: 10px 0;
}

nav {
  display: flex;
  justify-content: center;
  gap: 20px;
}

nav a {
  text-decoration: none;
  color: #2c3e50;
  font-weight: bold;
}

nav a:hover {
  color: #42b983;
}

main {
  margin-top: 20px;
}
</style>
  