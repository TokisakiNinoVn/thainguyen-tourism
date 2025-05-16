<!-- CustomerAdd.vue -->
<template>
    <div v-if="show" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
      <div class="bg-white p-4 rounded-lg shadow-lg w-1/2">
        <h2 class="text-xl font-bold mb-4">Thêm khách hàng</h2>
        <form @submit.prevent="handleAdd">
          <input v-model="formData.username" type="text" placeholder="Tên đăng nhập - Bắt buộc" class="border p-2 mb-2 w-full" required />
          <input v-model="formData.email" type="email" placeholder="Email - Bắt buộc" class="border p-2 mb-2 w-full" />
          <input v-model="formData.name" type="text" placeholder="Tên - Có thể để trống" class="border p-2 mb-2 w-full" required />
          <input v-model="formData.phone" type="text" placeholder="Điện thoại - Có thể để trống" class="border p-2 mb-2 w-full" />
          <input v-model="formData.address" type="text" placeholder="Địa chỉ - Có thể để trống" class="border p-2 mb-2 w-full" />
          <input v-model="formData.password" type="password" placeholder="Mật khẩu - Bắt buộc" class="border p-2 mb-2 w-full" />
          <select v-model="formData.gender" class="border p-2 mb-2 w-full">
            <option value="Nam">Nam</option>
            <option value="Nữ">Nữ</option>
            <option value="Khác">Khác</option>
          </select>
          <input v-model="formData.note" type="password" placeholder="Ghi chú - Không bắt buộc" class="border p-2 mb-2 w-full" />
          <button type="submit" class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">Thêm khách hàng </button>
          <button @click="close" type="button" class="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400 ml-2">Hủy</button>
        </form>
      </div>
    </div>
  </template>
  
  <script>
  import { ref } from 'vue';
  
  export default {
    props: ['show'],
    emits: ['close', 'addCustomer'],
    setup(props, { emit }) {
      const formData = ref({ username: '', email: '', name: '', phone: '', address: '', gender: 'Nam' });
  
      const handleAdd = () => {
        emit('addCustomer', formData.value);
        formData.value = {};
      };
  
      const close = () => {
        emit('close');
      };
  
      return { formData, handleAdd, close };
    },
  };
  </script>
  