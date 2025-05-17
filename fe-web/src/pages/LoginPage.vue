<template>
  <NavbarComponentV1 />
  <div
    class="relative flex items-center justify-center min-h-screen bg-cover bg-center"
    style="
      background-image: url('https://images.unsplash.com/photo-1507525428034-b723cf961d3e?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80');
    "
  >
    <!-- Overlay for better text readability -->
    <div class="absolute inset-0 bg-black/50"></div>

    <!-- Login Card -->
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
        Khám Phá Thái Nguyên
      </h2>
      <form @submit.prevent="handleLogin">
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
              v-model="email"
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
              v-model="password"
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
          Đăng Nhập
        </button>

        <!-- Google Login Button -->
        <button
          class="w-full bg-white border border-gray-300 hover:bg-gray-50 text-gray-800 font-semibold py-3 rounded-lg transition duration-300 mt-4 flex items-center justify-center gap-3"
          type="button"
          @click="handleGoogleLogin"
        >
          <img
            src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/google/google-original.svg"
            class="w-5 h-5"
          />
          Đăng nhập với Google
        </button>

        <!-- Forgot Password -->
        <button
          class="w-full text-green-600 hover:text-green-700 font-medium text-sm mt-4"
          @click="forgetPassword"
          type="button"
        >
          Quên mật khẩu?
        </button>

        <!-- Register Link -->
        <div class="mt-6 text-center text-sm">
          <p class="text-gray-600">
            Chưa có tài khoản?
            <router-link
              to="/register"
              class="text-green-600 hover:text-green-700 font-semibold"
            >
              Đăng ký ngay
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
import {
  login,
  saveInformation,
  loginGoogleApi,
} from "@/services/modules/auth.api";
import { auth, provider, signInWithPopup } from "@/firebase";

const email = ref("");
const password = ref("");
const errorMessage = ref("");
const router = useRouter();

const handleLogin = async () => {
  errorMessage.value = "";
  try {
    const response = await login({
      email: email.value,
      password: password.value,
    });
    const { data } = response.data;
    localStorage.setItem("isLoggedIn", "true");
    localStorage.setItem("user", JSON.stringify(data));
    localStorage.setItem("token", data.token);
    router.push("/");
  } catch (error) {
    console.error(error);
    errorMessage.value = error.response?.data?.message || "Đăng nhập thất bại";
  }
};

const handleGoogleLogin = async () => {
  try {
    const result = await signInWithPopup(auth, provider);
    const user = result.user;
    const isNewUser =
      user.metadata.creationTime === user.metadata.lastSignInTime;
    const userData = {
      email: user.email,
      name: user.displayName,
      avatar: user.photoURL,
      uid: user.uid,
    };
    if (isNewUser) {
      await saveInformation(userData);
    }
    const response = await loginGoogleApi({ email: user.email });
    const { data } = response.data;
    localStorage.setItem("token", data.token);
    localStorage.setItem("isLoggedIn", "true");
    localStorage.setItem("user", JSON.stringify(userData));
    router.push("/");
  } catch (error) {
    console.error("Google login error:", error);
    errorMessage.value = "Đăng nhập Google thất bại";
  }
};

const forgetPassword = () => {
  alert("Chức năng đang được phát triển! 🥲");
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
