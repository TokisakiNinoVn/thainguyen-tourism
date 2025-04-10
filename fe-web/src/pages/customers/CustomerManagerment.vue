<template>
  <div class="flex w-screen h-screen bg-gray-100">
    <Navbar />
    <div class="flex flex-col w-full p-6">
      <div class="flex flex-col mb-4">
        <h1 class="text-2xl font-bold mb-2">Quản lý khách hàng</h1>
        <div class="flex items-center mb-2">
          <input
            type="text"
            v-model="searchQuery"
            placeholder="Nhập thông tin cần tìm kiếm..."
            class="border border-gray-300 rounded-lg p-2 mr-2 flex-grow focus:outline-none focus:ring focus:ring-yellow-500"
          />
          <button 
            class="bg-blue-500 text-white rounded-lg px-4 py-2 hover:bg-blue-600 transition" 
            @click="searchCustomer" 
            :disabled="isMiniLoading || !searchQuery">
            <ion-icon name="search-outline"></ion-icon>
          </button>
        </div>
        <div class="flex items-center mb-4">
          <select v-model="selectedRole" class="border border-gray-300 rounded-lg p-2 mr-2 focus:outline-none focus:ring focus:ring-yellow-500">
            <option value="khách hàng">Khách hàng</option>
            <option value="nhân viên">Nhân viên</option>
          </select>
          <button 
            class="bg-blue-500 text-white flex items-center rounded-lg px-4 py-2 hover:bg-blue-600 transition" 
            @click="filterByRoleD(selectedRole)"
            :disabled="isMiniLoading">
            <ion-icon class="mr-2" name="search-outline"></ion-icon>
            Lọc
          </button>
          <button @click="resetFilters" class="flex items-center bg-gray-300 text-gray-700 rounded-lg px-4 py-2 hover:bg-gray-400 transition ml-2">
            <ion-icon class="mr-1" name="refresh-outline"></ion-icon>
            Làm mới bảng
          </button>
        </div>
      </div>

      <div class="overflow-x-auto">
        <button @click="showAddForm" class="bg-green-500 text-white rounded-lg px-4 py-2 mb-4 hover:bg-green-600 transition">
          <ion-icon name="add-outline"></ion-icon> Thêm khách hàng
        </button>
        <table v-if="customers.length > 0" class="min-w-full border border-gray-300 bg-white shadow-md rounded-lg">
          <thead class="bg-gray-100">
            <tr>
              <th class="border-b border-gray-300 p-2 text-left">ID</th>
              <th class="border-b border-gray-300 p-2 text-left">Tên đăng nhập</th>
              <th class="border-b border-gray-300 p-2 text-left">Tên</th>
              <th class="border-b border-gray-300 p-2 text-left">Email</th>
              <th class="border-b border-gray-300 p-2 text-left">Điện thoại</th>
              <th class="border-b border-gray-300 p-2 text-left">Địa chỉ</th>
              <th class="border-b border-gray-300 p-2 text-left">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="customer in customers" :key="customer.id" class="hover:bg-gray-50">
              <td class="border-b border-gray-300 p-2">{{ customer.id || '' }}</td>
              <td class="border-b border-gray-300 p-2">{{ customer.username || '' }}</td>
              <td class="border-b border-gray-300 p-2">{{ customer.name || '' }}</td>
              <td class="border-b border-gray-300 p-2">{{ customer.email || '' }}</td>
              <td class="border-b border-gray-300 p-2">{{ customer.phone || '' }}</td>
              <td class="border-b border-gray-300 p-2">{{ customer.address || '' }}</td>
              <td class="border-b border-gray-300 p-2">
                <button @click="showUpdateForm(customer)" class="text-yellow-500 hover:text-yellow-700 transition" title="Cập nhật">
                  <ion-icon class="text-2xl p-1 hover:bg-yellow-400" name="pencil-outline"></ion-icon>
                </button>
                <button @click="viewCustomer(customer)" class="text-blue-500 hover:text-blue-700 transition" title="Xem chi tiết">
                  <ion-icon name="eye-outline"></ion-icon>
                </button>
                <button @click="removeCustomer(customer.id)" class="text-red-500 hover:text-red-700 transition" title="Xóa">
                  <ion-icon name="trash-outline"></ion-icon>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
        <div v-else class="text-center mt-4">
          Không có dữ liệu!
        </div>
      </div>
    </div>
  </div>
  <div v-if="showDetail" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
    <CustomerDetail :customer="customerDetail" @close="closeDetail" />
  </div>
  <CustomerAdd :show="showAddModal" @close="closeAddModal" @addCustomer="addCustomer" />
  <CustomerUpdate :show="showUpdateModal" :customer="selectedCustomer" @close="closeUpdateModal" @updateCustomer="updateCustomer" />

</template>


<script setup>
import { ref, onBeforeMount } from 'vue';
import { useRouter } from 'vue-router';
import Navbar from '../../components/NavbarComponent.vue';
import CustomerDetail from './CustomerDetail.vue';
import CustomerAdd from './CustomerAdd.vue';
import CustomerUpdate from './CustomerUpdate.vue';
import {
  getAll,
  add,
  update,
  // search,
  remove,
  getById,
  filterByRole
} from '@/services/modules/user.api';

const router = useRouter();

const customers = ref([]);
const isMiniLoading = ref(false);
const searchQuery = ref('');
const selectedRole = ref('khách hàng');
const showDetail = ref(false);
const customerDetail = ref(null);
const showAddModal = ref(false);
const showUpdateModal = ref(false);
const selectedCustomer = ref(null);

const showAddForm = () => {
  showAddModal.value = true;
};

const closeAddModal = () => {
  showAddModal.value = false;
};

const closeUpdateModal = () => {
  showUpdateModal.value = false;
};

onBeforeMount(() => {
  fetchCustomers();
});

const fetchCustomers = async () => {
  isMiniLoading.value = true;
  try {
    const response = await getAll();
    if (Array.isArray(response.data)) {
      customers.value = response.data;
    } else {
      throw new Error('Dữ liệu không phải là một mảng');
    }
  } catch (error) {
    console.error('Error fetching customers:', error);
    alert('Phiên đăng nhập hết hạn, vui lòng đăng nhập lại! 🥲');
    customers.value = [];
    setTimeout(() => {
        router.push('/login');
    }, 2000);
  } finally {
    isMiniLoading.value = false;
  }
};

const resetFilters = () => {
  searchQuery.value = '';
  selectedRole.value = 'khách hàng';
  fetchCustomers();
};


// Xóa khách hàng theo ID
const removeCustomer = async (id) => {
  const confirmDelete = confirm('Bạn có chắc chắn muốn xóa khách hàng này?');
  if (confirmDelete) {
    try {
      await remove(id);
      customers.value = customers.value.filter(customer => customer.id !== id);
      alert('Khách hàng đã được xóa!');
    } catch (error) {
      console.error('Error removing customer:', error);
      alert('Có lỗi xảy ra khi xóa khách hàng. Vui lòng thử lại.');
    }
  }
};

// Xem chi tiết customer
const viewCustomer = async (customer) => {
  try {
    const response = await getById(customer.id);
    if (response.data) {
      customerDetail.value = response.data;
      showDetail.value = true; // Show the detail modal
    } else {
      throw new Error('Không tìm thấy thông tin khách hàng');
    }
  } catch (error) {
    console.error('Error fetching customer details:', error);
    alert('Có lỗi xảy ra khi xem chi tiết khách hàng.');
  }
};

const closeDetail = () => {
  showDetail.value = false;
  customerDetail.value = null;
};

const filterByRoleD = async (role) => {
  console.log('Filtering by role:', role);
  isMiniLoading.value = true;

  try {
    const filteredCustomers = await filterByRole(role);
    if (Array.isArray(filteredCustomers.data)) {
      customers.value = filteredCustomers.data;
    } else {
      throw new Error('Dữ liệu không phải là một mảng');
    }
  } catch (error) {
    console.error('Error filtering customers:', error);
    alert('Không thể lọc thông tin khách hàng. Vui lòng thử lại sau.');
    customers.value = []; // Reset customers on error
  } finally {
    isMiniLoading.value = false;
  }
};

const searchCustomer = async () => {
  if (!searchQuery.value) {
    alert('Vui lòng nhập thông tin cần tìm kiếm.');
    return;
  }

  const filteredCustomers = customers.value.filter(customer => 
    customer.username?.includes(searchQuery.value) ||
    customer.name?.includes(searchQuery.value) ||
    customer.email?.includes(searchQuery.value) ||
    customer.phone?.includes(searchQuery.value) ||
    customer.address?.includes(searchQuery.value)
  );

  customers.value = filteredCustomers.length ? filteredCustomers : [];
  if (filteredCustomers.length === 0) {
    alert('Không tìm thấy khách hàng nào.');
  }
};

const addCustomer = async (customerData) => {
  try {
    await add(customerData); // Gọi API thêm khách hàng
    alert('Thêm khách hàng thành công!');
    fetchCustomers(); // Tải lại danh sách khách hàng
    closeAddModal();
  } catch (error) {
    console.error('Error adding customer:', error);
    alert('Có lỗi xảy ra khi thêm khách hàng.');
  }
};

const showUpdateForm = (customer) => {
  selectedCustomer.value = customer;
  showUpdateModal.value = true;
};

const updateCustomer = async (updatedData) => {
  try {
    await update(updatedData.id, updatedData); // Gọi API cập nhật khách hàng
    alert('Cập nhật khách hàng thành công!');
    fetchCustomers();
    closeUpdateModal();
  } catch (error) {
    console.error('Error updating customer:', error);
    alert('Có lỗi xảy ra khi cập nhật khách hàng.');
  }
};
</script>


<style scoped>
/* Bỏ qua CSS riêng biệt vì đã sử dụng Tailwind */
</style>
