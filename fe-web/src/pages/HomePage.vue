<template>
  <div class="home-page">
    <!-- Navbar điều hướng -->
    <Navbar />
    <!-- Nội dung chính của trang Home -->
    <section class="main-content">
      <h1 class="hello"> Chào mừng đến với Hệ Thống Quản Lý Khách Hàng của Bưu Điện tỉnh Thái Nguyên</h1>
      
      <div class="banner">
        <img src="../assets/banner1.jpg" alt="Banner">
      </div>
      <div class="news-component">
        <News />
      </div>
    </section>
  </div>
</template>

<script setup>
import Navbar from '../components/NavbarComponent.vue';
import News from '../components/NewComponent.vue';
</script>

<style scoped>
.home-page {
  width: 100vw;
  height: 100vh;
  display: flex;
  overflow: visible;
}

.main-content {
  font-family: Arial, sans-serif;
  padding: 20px;
  padding-bottom: 0;
  display: flex;
  flex-direction: column;
  width: 100%;
  /* height: 200vh; */
}

.hello {
  font-size: 30px;
  margin: 20px auto 20px auto;
  font-weight: bold;
}

.banner {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.news-component {
  overflow: scroll;
  margin-top: 20px;
  padding-bottom: 0;
}
</style>
