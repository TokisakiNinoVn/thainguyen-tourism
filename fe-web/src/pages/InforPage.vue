<template>
  <div class="full-page">
    <div class="info-page my-5 p-5 bg-gray-100 rounded-lg shadow-lg" v-if="user">
      <h2 class="title text-2xl font-bold text-center mb-5">Thông tin nhân viên</h2>
      <div class="main-infor">
        <div class="infor-user flex">
          <a href="https://nino.is-a.dev/" class="link-dev flex justify-center">
            <img src="../assets/avtart-default.jpg" alt="Avatar nhân viên" class="avatar h-48 w-48 rounded-full border-4 border-transparent hover:border-pink-500 transition-all duration-200">
          </a>
          <div class="infor-text ml-4">
            <div v-if="user.id" class="info-item text-lg my-2">
              <strong>ID Nhân viên:</strong> <span>[{{ user.id }}]</span>
            </div>
            <div v-if="user.name" class="info-item text-lg my-2">
              <strong>Tên:</strong> <span>{{ user.name }}</span>
            </div>
            <div v-if="user.email" class="info-item text-lg my-2">
              <strong>Email:</strong> <span>{{ user.email }}</span>
            </div>
            <div v-if="user.gender" class="info-item text-lg my-2">
              <strong>Giới tính:</strong> <span>{{ user.gender }}</span>
            </div>
            <div v-if="user.role" class="info-item text-lg my-2">
              <strong>Vai trò:</strong> <span>{{ user.role }}</span>
            </div>
            <div v-if="user.phone" class="info-item text-lg my-2">
              <strong>Số điện thoại:</strong> <span>{{ user.phone }}</span>
            </div>
            <div v-if="user.address" class="info-item text-lg my-2">
              <strong>Địa chỉ:</strong> <span>{{ user.address }}</span>
            </div>
            <div v-if="user.updatedAt" class="info-item text-lg my-2">
              <strong>Cập nhật lần cuối:</strong> <span>{{ formattedDate }}</span>
            </div>
          </div>
        </div>
        <div class="nav-link mt-5">
          <nav class="flex">
            <router-link to="/home" class="flex items-center my-3 text-black hover:text-yellow-500">
              <ion-icon name="chevron-back-outline" class="text-lg"></ion-icon>
              <span class="ml-2">Quay lại</span>
            </router-link>
            <button @click.prevent="updateInfor" class="update-infor flex items-center mx-3 px-4 py-2 bg-yellow-500 text-white rounded hover:bg-yellow-600 transition duration-200">
              <ion-icon name="paw-outline" class="text-lg"></ion-icon>
              <span class="ml-2">Cập nhật thông tin</span>
            </button>
            <a class="item-navbar text-black flex items-center cursor-pointer hover:text-yellow-600" @click.prevent="handleLogout">
              <ion-icon name="log-out-outline" class="text-lg"></ion-icon>
              <span class="ml-2">Đăng xuất</span>
            </a>
          </nav>
        </div>
      </div>
    </div>
  
    <div v-else>
      <p class="text-center text-lg">Không có thông tin nhân viên nào được tìm thấy.</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

const user = ref(null);
const formattedDate = ref('');

// Fetch user data from localStorage when the component is mounted
onMounted(() => {
  const storedUser = JSON.parse(localStorage.getItem('user'));
  if (storedUser) {
    user.value = storedUser;
    // Format the updatedAt date
    formattedDate.value = new Date(storedUser.updatedAt).toLocaleString();
  } else {
    user.value = {
      id: 'N/A',
      name: 'N/A',
      email: 'N/A',
      role: 'N/A',
      phone: 'N/A',
      address: 'N/A',
      updatedAt: 'N/A',
    };
  }
});

const handleLogout = () => {
  // Clear localStorage data
  localStorage.removeItem('isLoggedIn');
  localStorage.removeItem('user');
  
  // Set isLoggedIn to false
  localStorage.setItem('isLoggedIn', 'false');
  
  // Redirect to login page
  router.push('/login');
};
const updateInfor = () => {
  alert("Chức năng đang được phát triển! 🥲")
};


</script>

<style scoped>
.info-item {
  text-align: left;
}

.info-page {
  width: max-content;
  padding: 20px 40px;
}

.link-dev {
  align-items: center;
  margin-right: 20px;
}

.full-page {
  width: 100vw;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
