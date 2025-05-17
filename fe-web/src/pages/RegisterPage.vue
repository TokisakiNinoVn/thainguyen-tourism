<template>
  <NavbarComponentV1 />
  <div
    class="relative flex items-center justify-center min-h-screen bg-cover bg-center"
    style="
      background-image: url('https://images.unsplash.com/photo-1507525428034-b723cf961d3e?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80');
    "
  >
    <!-- Overlay for readability -->
    <div class="absolute inset-0 bg-black/50"></div>

    <!-- Register Card -->
    <div
      class="relative bg-white/95 backdrop-blur-sm shadow-xl rounded-2xl p-10 max-w-md w-full transform transition-all duration-500 hover:scale-105"
    >
      <div class="flex justify-center mb-6">
        <img
          src="https://i.pinimg.com/736x/34/f7/00/34f700d8069a416f56cd7ed40cc35eb6.jpg"
          alt="Thái Nguyên Logo"
          class="h-24 w-24 object-contain rounded-full shadow-lg"
        />
      </div>
      <h2
        class="text-3xl font-bold mb-8 text-center text-green-800 tracking-tight"
      >
        Tham Gia Khám Phá Thái Nguyên
      </h2>
      <form @submit.prevent="handleRegister">
        <!-- Display Name Field -->
        <div class="mb-6">
          <label
            for="displayName"
            class="block text-sm font-medium text-gray-700 mb-2"
          >
            Tên hiển thị
          </label>
          <div class="relative">
            <input
              type="text"
              id="displayName"
              v-model="inforRegister.displayName"
              required
              class="block w-full pl-10 pr-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition duration-200"
              placeholder="Nguyễn Văn A"
            />
            <svg
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"
              />
            </svg>
          </div>
        </div>

        <!-- Email Field -->
        <div class="mb-6">
          <label
            for="email"
            class="block text-sm font-medium text-gray-700 mb-2"
          >
            Email
          </label>
          <div class="relative">
            <input
              type="email"
              id="email"
              v-model="inforRegister.email"
              required
              class="block w-full pl-10 pr-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition duration-200"
              placeholder="nhap@email.com"
            />
            <svg
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M16 12a4 4 0 10-8 0 4 4 0 008 0zm0 0v1.5a2.5 2.5 0 005 0V12a9 9 0 10-9 9m4.5-1.206a8.959 8.959 0 01-4.5 1.207"
              />
            </svg>
          </div>
        </div>

        <!-- Password Field -->
        <div class="mb-6">
          <label
            for="password"
            class="block text-sm font-medium text-gray-700 mb-2"
          >
            Mật khẩu
          </label>
          <div class="relative">
            <input
              type="password"
              id="password"
              v-model="inforRegister.password"
              required
              class="block w-full pl-10 pr-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition duration-200"
              placeholder="••••••••"
            />
            <svg
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M12 11c0-1.1.9-2 2-2s2 .9 2 2-2 4-2 4m-2-6c0-1.1-.9-2-2-2s-2 .9-2 2m2 6v4m6-10h2m-2 4h2m-8-4H4m2 4H4"
              />
            </svg>
          </div>
        </div>

        <!-- Submit Button -->
        <button
          type="submit"
          class="w-full bg-green-600 hover:bg-green-700 text-white font-semibold py-3 rounded-lg transition duration-300 transform hover:-translate-y-1"
        >
          Đăng Ký
        </button>

        <!-- Login Link -->
        <div class="mt-6 text-center text-sm">
          <p class="text-gray-600">
            Đã có tài khoản?
            <router-link
              to="/login"
              class="text-green-600 hover:text-green-700 font-semibold"
            >
              Đăng nhập
            </router-link>
          </p>
        </div>

        <!-- Google Login Link -->
        <div class="mt-4 text-center text-sm">
          <p class="text-gray-600">
            Hoặc đăng nhập bằng
            <router-link
              to="/login"
              class="text-green-600 hover:text-green-700 font-semibold"
            >
              Google
            </router-link>
          </p>
        </div>

        <!-- Error Message -->
        <div
          v-if="errorMessage"
          class="mt-4 text-red-600 text-sm text-center animate-pulse"
        >
          {{ errorMessage }}
        </div>
      </form>
    </div>
  </div>
  <FooterComponent />
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import NavbarComponentV1 from "@/components/NavbarComponentV1.vue";
import FooterComponent from "@/components/FooterComponent.vue";
import { registerApi } from "@/services/modules/auth.api";

const router = useRouter();
const errorMessage = ref("");

// Dữ liệu form đăng ký
const inforRegister = ref({
  displayName: "",
  email: "",
  password: "",
});

// Xử lý đăng ký
const handleRegister = async () => {
  errorMessage.value = ""; // reset lỗi

  try {
    const response = await registerApi(inforRegister.value);

    if ((response && response.status === 201) || response.status === 200) {
      alert("Đăng ký thành công!");
      router.push("/login");
    } else {
      errorMessage.value = "Đăng ký không thành công. Vui lòng thử lại.";
    }
  } catch (error) {
    // Hiển thị lỗi từ server nếu có
    errorMessage.value =
      error.response?.data?.data?.message || "Đã xảy ra lỗi. Vui lòng thử lại.";
    console.error("Đăng ký thất bại:", error.response?.data?.data?.message);
  }
};
</script>

<style scoped>
/* Custom Animations */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

input:focus {
  outline: none;
}

form {
  animation: fadeIn 0.6s ease-out;
}

/* Responsive Design */
@media (max-width: 640px) {
  .p-10 {
    padding: 2rem;
  }
  .max-w-md {
    max-width: 90%;
  }
}
</style>
