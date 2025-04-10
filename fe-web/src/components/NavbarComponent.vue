<template>
  <div class="main-navbar">
    <nav class="navbar">
      <router-link class="flex item-center flex-col" to="/home">
        <img class="image-logo" src="../assets/logo-4-3.png" alt="Logo">
      </router-link>
      <router-link class="item-navbar" to="/create-order">
        <ion-icon class="icon-item-navbar" name="document-outline"></ion-icon>
        Tạo đơn hàng mới
      </router-link>
      <router-link class="item-navbar" to="/orders">
        <ion-icon class="icon-item-navbar" name="reader-outline"></ion-icon>
        Tra cứu đơn hàng
      </router-link>
      <router-link class="item-navbar" to="/customer-list">
        <ion-icon name="people-circle-outline"></ion-icon>
        Quản lý khách hàng
      </router-link>
      <router-link class="item-navbar" to="/transaction-history">
        <ion-icon class="icon-item-navbar" name="sync-outline"></ion-icon>
        Lịch sử giao dịch
      </router-link>
      <router-link class="item-navbar" to="/vouchers">
        <ion-icon class="icon-item-navbar" name="ticket-outline"></ion-icon>
        Quản Lý voucher
      </router-link>
      <router-link class="item-navbar" to="/chat">
        <ion-icon class="icon-item-navbar" name="chatbubbles-outline"></ion-icon>
        Chat
      </router-link>
      <router-link class="item-navbar" to="/notifications">
        <ion-icon class="icon-item-navbar" name="notifications-outline"></ion-icon>
        Quản lý thông báo
      </router-link>
      <router-link class="item-navbar" to="/complaints-management">
        <ion-icon class="icon-item-navbar" name="mail-outline"></ion-icon>
        Quản lý khiếu nại
      </router-link>
      <hr>
      <router-link class="item-navbar" to="/user">
        <ion-icon name="person-circle-outline"></ion-icon>
        Thông tin nhân viên
      </router-link>
      <a class="item-navbar" @click.prevent="handleLogout" href="#">
        <ion-icon name="log-out-outline"></ion-icon>
        Đăng xuất
      </a>
    </nav>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';

const router = useRouter();

const handleLogout = () => {
  // Clear localStorage data
  localStorage.removeItem('isLoggedIn');
  localStorage.removeItem('user');
  
  // Set isLoggedIn to false
  localStorage.setItem('isLoggedIn', 'false');
  
  // Redirect to login page
  router.push('/login');
};
</script>

<style scoped>
.main-navbar {
  position: relative;
  width: auto;
  background-color: #f1f1f1;
  height: 100vh;
}
.navbar {
  display: flex;
  flex-direction: column;
  padding: 10px;
  justify-content: flex-start;
  width: max-content;
}
.navbar a {
  color: rgb(0, 0, 0);
  text-decoration: none;
  padding: 8px 12px;
  border-radius: 4px;
  border-bottom: 1px solid rgba(0, 0, 0, 0);
}
.navbar a:hover {
  background-color: #ffffff;
  border-bottom: 1px solid rgb(255, 170, 0);
  color: rgb(255, 170, 0);
}
.image-logo {
  height: 100px;
  border-radius: 20px;
}
.item-navbar {
  display: flex;
  justify-content: flex-start;
  align-items: center;
}
ion-icon {
  margin-right: 8px;
}
hr {
  height: 2px; /* Độ dày của thanh ngang */
  background-color: rgb(255, 170, 0); /* Màu sắc của thanh ngang */
  border: none; /* Xóa bỏ đường viền mặc định */
}
</style>
