<template>
  <div class="flex w-screen h-screen bg-gray-100">
    <Navbar />
    <div class="flex flex-col w-full p-8 space-y-6">
      <h2 class="text-3xl font-semibold text-gray-800">Quản lý các khiếu nại khách hàng</h2>
      <div class="flex items-center space-x-4">
        <input
          type="text"
          v-model="searchQuery"
          placeholder="Nhập thông tin cần tìm kiếm..."
          class="border border-gray-300 rounded-lg p-3 flex-grow focus:outline-none focus:ring focus:ring-yellow-500 transition duration-150"
        />
        <button 
          class="bg-blue-600 text-white rounded-lg px-4 py-2 hover:bg-blue-700 transition duration-150" 
          @click="search" 
          :disabled="isMiniLoading || !searchQuery">
          <ion-icon name="search-outline"></ion-icon>
        </button>
      </div>
      <div class="flex space-x-2">
        <button 
          class="bg-blue-600 text-white flex items-center rounded-lg px-4 py-2 hover:bg-blue-700 transition duration-150" 
          :disabled="isMiniLoading"
          @click="filterComplaints">
          <ion-icon class="mr-2" name="filter-outline"></ion-icon>
          Lọc
        </button>
        <button 
          @click="resetFilters" 
          class="flex items-center bg-gray-300 text-gray-700 rounded-lg px-4 py-2 hover:bg-gray-400 transition duration-150">
          <ion-icon class="mr-1" name="refresh-outline"></ion-icon>
          Làm mới
        </button>
      </div>

      <div class="overflow-x-auto">
        <table class="min-w-full bg-white border border-gray-200">
          <thead>
            <tr>
              <th class="py-2 px-4 border-b-2 border-gray-200 text-left text-gray-700">Tên khách hàng</th>
              <th class="py-2 px-4 border-b-2 border-gray-200 text-left text-gray-700">Mô tả</th>
              <th class="py-2 px-4 border-b-2 border-gray-200 text-left text-gray-700">Trạng thái</th>
              <th class="py-2 px-4 border-b-2 border-gray-200 text-left text-gray-700">Ngày tạo</th>
              <th class="py-2 px-4 border-b-2 border-gray-200 text-left text-gray-700">Phản hồi</th>
              <th class="py-2 px-4 border-b-2 border-gray-200 text-left text-gray-700">Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="complaint in complaints" :key="complaint.id" class="hover:bg-gray-100 transition duration-150">
              <td class="py-4 px-4 border-b border-gray-200">{{ complaint.name }} ({{ complaint.username }})</td>
              <td class="py-4 px-4 border-b border-gray-200">{{ complaint.description }}</td>
              <td v-if="complaint.status === 'new'" class="py-4 px-4 border-b border-gray-200">
                <span>Mới</span>
              </td>
              <td v-else-if="complaint.status === 'responded'" class="py-4 px-4 border-b border-gray-200">
                <span>Đã có phản hồi</span>
              </td>
              <td v-else class="py-4 px-4 border-b border-gray-200">
                <span>Không xác định</span>
              </td>

              <td class="py-4 px-4 border-b border-gray-200">{{ new Date(complaint.createdAt).toLocaleString() }}</td>
              <td class="py-4 px-4 border-b border-gray-200">
                <textarea 
                  v-model="complaint.response" 
                  placeholder="Nhập phản hồi ở đây..." 
                  class="border border-gray-300 rounded-lg p-1 w-full resize-none"
                  rows="3"></textarea>
              </td>
              <td class="py-4 px-4 border-b border-gray-200">
                <button 
                  class="bg-green-600 text-white rounded-lg px-4 py-2 hover:bg-green-700 transition duration-150" 
                  @click="() => addResponse(complaint.id, complaint.response)"
                  :disabled="isMiniLoading || !complaint.response || complaint.status === 'responded'">
                  Gửi phản hồi
                </button>

              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onBeforeMount } from 'vue';
import { getAll } from '@/services/modules/complaint.api';
import { add } from '@/services/modules/response.api';
import Navbar from '@/components/NavbarComponent.vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const complaints = ref([]);
const isMiniLoading = ref(false);
const searchQuery = ref('');

// Gọi API để lấy dữ liệu các khiếu nại
onBeforeMount(() => {
  fetchComplaints();
});

// Tải các khiếu nại từ API
const fetchComplaints = async () => {
  isMiniLoading.value = true;
  try {
    const response = await getAll();
    if (Array.isArray(response.data)) {
      complaints.value = response.data.map(complaint => ({ ...complaint, response: '' }));
    } else {
      throw new Error('Dữ liệu không phải là một mảng');
    }
  } catch (error) {
    console.error('Error fetching complaints:', error);
    alert('Phiên đăng nhập hết hạn, vui lòng đăng nhập lại! 🥲');
    complaints.value = [];
    setTimeout(() => {
      router.push('/login');
    }, 2000);
  } finally {
    isMiniLoading.value = false;
  }
};

// Thêm phản hồi cho khiếu nại
const addResponse = async (complaintId, responseText) => {
  if (!responseText) return; // Không gửi nếu không có phản hồi
  isMiniLoading.value = true;
  try {
    await add({ complaintId, responseText }); // Gọi API để thêm phản hồi
    alert('Phản hồi đã được gửi thành công!');

    // Cập nhật trạng thái phản hồi
    const complaint = complaints.value.find(c => c.id === complaintId);
    if (complaint) {
      complaint.hasResponded = true; // Đánh dấu là đã phản hồi
    }

    fetchComplaints(); // Tải lại danh sách khiếu nại để cập nhật
  } catch (error) {
    console.error('Error sending response:', error);
    alert('Có lỗi xảy ra khi gửi phản hồi.');
  } finally {
    isMiniLoading.value = false;
  }
};

// Phương thức tìm kiếm
const search = () => {
  console.log('Searching for:', searchQuery.value);
  // Kiểm tra xem có giá trị trong searchQuery hay không
  if (searchQuery.value) {
    // Lọc danh sách khiếu nại dựa trên tên khách hàng hoặc mô tả
    complaints.value = complaints.value.filter(complaint =>
      complaint.name.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      complaint.description.toLowerCase().includes(searchQuery.value.toLowerCase())
    );
  } else {
    // Nếu searchQuery trống, gọi lại dữ liệu ban đầu
    fetchComplaints();
  }
};

// Phương thức lọc các khiếu nại
const filterComplaints = () => {
  console.log('Filtering complaints...');
  // Kiểm tra trạng thái hiện tại của khiếu nại
  const currentStatus = complaints.value.some(complaint => complaint.status === 'new') ? 'responded' : 'new';

  // Lọc các khiếu nại dựa trên trạng thái
  if (currentStatus === 'new') {
    complaints.value = complaints.value.filter(complaint => complaint.status === 'new');
  } else {
    complaints.value = complaints.value.filter(complaint => complaint.status === 'responded');
  }

  // Nếu không có khiếu nại nào, reset lại danh sách
  if (complaints.value.length === 0) {
    alert('Không có khiếu nại nào để hiển thị.');
    fetchComplaints();
  }
};

// Phương thức làm mới các bộ lọc
const resetFilters = () => {
  console.log('Resetting filters...');
  // Đặt lại searchQuery về giá trị rỗng
  searchQuery.value = '';
  // Gọi lại dữ liệu ban đầu để hiển thị tất cả các khiếu nại
  fetchComplaints();
};

</script>


<style scoped>
.complaint-item {
  border-bottom: 1px solid #ddd;
  padding: 10px 0;
}
textarea {
  width: 100%;
  margin: 5px 0;
}
button {
  padding: 5px 10px;
  background-color: #28a745;
  color: white;
  border: none;
  cursor: pointer;
}
button:hover {
  background-color: #218838;
}
</style>
