<template>
    <div v-if="customer" class="flex flex-col bg-white p-4 rounded-lg shadow-md w-1/4">
      <h2 class=" text-xl font-bold mb-4 bg-yellow-500 p-2">Thông tin chi tiết khách hàng</h2>
      <table class="w-full border-collapse">
        <tbody>
            <tr>
                <td class="text-right p-2 border border-gray-300"><strong>ID:</strong></td>
                <td class="text-left p-2 border border-gray-300">{{ customer.id }}</td>
            </tr>
            <tr>
                <td class="text-right p-2 border border-gray-300"><strong>Tên đăng nhập:</strong></td>
                <td class="text-left p-2 border border-gray-300">{{ customer.username }}</td>
            </tr>
            <tr>
                <td class="text-right p-2 border border-gray-300"><strong>Tên:</strong></td>
                <td class="text-left p-2 border border-gray-300">{{ customer.name }}</td>
            </tr>
            <tr>
                <td class="text-right p-2 border border-gray-300"><strong>Email:</strong></td>
                <td class="text-left p-2 border border-gray-300">{{ customer.email }}</td>
            </tr>
            <tr>
                <td class="text-right p-2 border border-gray-300"><strong>Điện thoại:</strong></td>
                <td class="text-left p-2 border border-gray-300">{{ customer.phone }}</td>
            </tr>
            <tr>
                <td class="text-right p-2 border border-gray-300"><strong>Địa chỉ:</strong></td>
                <td class="text-left p-2 border border-gray-300">{{ customer.address }}</td>
            </tr>
            <tr>
                <td class="text-right p-2 border border-gray-300"><strong>Giới tính:</strong></td>
                <td class="text-left p-2 border border-gray-300">{{ customer.gender }}</td>
            </tr>
            <tr>
                <td class="text-right p-2 border border-gray-300"><strong>Ghi chú:</strong></td>
                <td class="text-left p-2 border border-gray-300">{{ customer.note }}</td>
            </tr>
          </tbody>
      </table>
      <button @click="closeDetail" class="mt-4 bg-yellow-500 text-black px-4 py-2 rounded hover:bg-yellow-600">
        Đóng
      </button>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      customer: {
        type: Object,
        required: true,
      },
    },
    emits: ['close'],
    methods: {
      closeDetail() {
        this.$emit('close');
      },
    },
  };
  </script>
  
  <style scoped>
  /* Add custom styles here if needed */
  p {
    text-align: left;
  }
  </style>
  