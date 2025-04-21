<template>
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="bg-white shadow-md rounded-lg p-8 max-w-sm w-full">
      <h2 class="text-2xl font-bold mb-6 text-center">Đăng nhập</h2>
      <form @submit.prevent="handleLogin">
        <div class="mb-4">
          <label for="email" class="block text-sm font-medium text-gray-700">Email:</label>
          <input
            type="email"
            id="email"
            v-model="email"
            required
            class="mt-1 block w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-500"
          />
        </div>
        <div class="mb-4">
          <label for="password" class="block text-sm font-medium text-gray-700">Mật khẩu:</label>
          <input
            type="password"
            id="password"
            v-model="password"
            required
            class="mt-1 block w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-500"
          />
        </div>
        <button
          type="submit"
          class="w-full bg-blue-500 hover:bg-blue-600 text-white font-semibold py-2 rounded-md transition duration-200"
        >
          Đăng nhập
        </button>

        <button
          class="w-full bg-red-500 hover:bg-red-600 text-white font-semibold py-2 rounded-md transition duration-200 mt-3 flex items-center justify-center gap-2"
          type="button"
          @click="handleGoogleLogin"
        >
          <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/google/google-original.svg" class="w-5 h-5" />
          Đăng nhập với Google
        </button>

        <button
          class="w-full bg-yellow-200 hover:bg-yellow-500 text-black font-semibold py-2 rounded-md transition duration-200 mt-2"
          @click="forgetPassword"
          type="button"
        >
          Quên mật khẩu
        </button>

        <div v-if="errorMessage" class="mt-4 text-red-600 text-sm text-center">{{ errorMessage }}</div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { login, saveInformation } from '@/services/modules/auth.api';
import { auth, provider, signInWithPopup } from '@/firebase';

const email = ref('');
const password = ref('');
const errorMessage = ref('');

const router = useRouter();

const handleLogin = async () => {
  errorMessage.value = '';
  try {
    const response = await login({ email: email.value, password: password.value });

    const { data } = response.data;
    localStorage.setItem('isLoggedIn', 'true');
    localStorage.setItem('user', JSON.stringify(data));

    router.push('/');
  } catch (error) {
    console.error(error);
    errorMessage.value = error.response?.data?.message || 'Đăng nhập thất bại';
  }
};

const handleGoogleLogin = async () => {
  try {
    const result = await signInWithPopup(auth, provider);
    const user = result.user;

    const isNewUser = user.metadata.creationTime === user.metadata.lastSignInTime;

    const userData = {
      email: user.email,
      name: user.displayName,
      avatar: user.photoURL,
      uid: user.uid,
    };

    // Chỉ gửi về backend nếu là lần đầu đăng nhập
    if (isNewUser) {
      await saveInformation(userData);
    }

    // console.log('Google login result:', userData);

    localStorage.setItem('isLoggedIn', 'true');
    localStorage.setItem('user', JSON.stringify(userData));

    router.push('/');
  } catch (error) {
    console.error('Google login error:', error);
    errorMessage.value = 'Đăng nhập Google thất bại';
  }
};

const forgetPassword = () => {
  alert('Chức năng đang được phát triển! 🥲');
};
</script>

<style scoped>
input:focus {
  outline: none;
}
</style>
