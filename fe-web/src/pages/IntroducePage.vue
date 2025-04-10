<template>
    <div id="app">
      <header>
        <nav>
          <a href="https://www.facebook.com/buudientinhTNN/">Trang chủ</a>
          <router-link to="/login">Đăng nhập</router-link>
        </nav>
      </header>
      <main>
      </main>
    </div>
  </template>
  
  <script>
  import { computed } from 'vue';
  import { useRouter } from 'vue-router';
  
  export default {
    name: 'App',
    components: {
      
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
  