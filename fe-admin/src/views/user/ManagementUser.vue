<template>
  <div class="py-4 container-fluid">
    <div class="row">
      <div class="col-12">
        <h3 class="text-center">Quản lý người dùng</h3>
      </div>
    </div>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-header">
            <h5 class="mb-0">Danh sách người dùng</h5>
          </div>
          <div class="card-body">
            <table class="table table-bordered table-hover">
              <thead class="thead-dark">
                <tr>
                  <th scope="col">STT</th>
                  <th scope="col">Tên hiển thị</th>
                  <th scope="col">Email</th>
                  <th scope="col">Ngày tạo</th>
                  <th scope="col">Trạng thái</th>
                  <th scope="col">Hành động</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(user, index) in listUser" :key="user.id">
                  <td>{{ index + 1 }}</td>
                  <td>{{ user.display_name }}</td>
                  <td>{{ user.email }}</td>
                  <td>{{ formatDate(user.created_at) }}</td>
                  <td>{{ user.status === 1 ? "Hoạt động" : "Bị khóa" }}</td>
                  <td>
                    <button
                      v-if="user.status === 1"
                      class="btn btn-sm btn-danger me-1"
                      @click="showConfirmModal(user.id, 'lock')"
                    >
                      Khóa
                    </button>
                    <button
                      v-else
                      class="btn btn-sm btn-success me-1"
                      @click="showConfirmModal(user.id, 'unlock')"
                    >
                      Mở khóa
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal xác nhận -->
    <div
      class="modal fade"
      id="confirmModal"
      tabindex="-1"
      aria-labelledby="confirmModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="confirmModalLabel">{{ modalTitle }}</h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body">
            {{ modalMessage }}
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-secondary"
              data-bs-dismiss="modal"
            >
              Hủy
            </button>
            <button
              type="button"
              class="btn"
              :class="modalAction === 'lock' ? 'btn-danger' : 'btn-success'"
              @click="confirmAction"
            >
              {{ modalAction === "lock" ? "Khóa" : "Mở khóa" }}
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { getListUser, lockUser, unlockUser } from "@/apis/modules/user.api";
import { ref, onMounted } from "vue";
import * as bootstrap from "bootstrap";

const listUser = ref([]);
const selectedUserId = ref(null);
const modalAction = ref("lock");
const modalTitle = ref("");
const modalMessage = ref("");

let confirmModal = null;

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString("vi-VN", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
};

const getListAllUser = async () => {
  try {
    const response = await getListUser();
    // Parse the stringified data
    listUser.value = JSON.parse(response.data);
  } catch (error) {
    console.error("Error fetching user list:", error);
  }
};

const showConfirmModal = (userId, action) => {
  selectedUserId.value = userId;
  modalAction.value = action;
  modalTitle.value =
    action === "lock"
      ? "Xác nhận khóa tài khoản"
      : "Xác nhận mở khóa tài khoản";
  modalMessage.value = `Bạn có chắc chắn muốn ${
    action === "lock" ? "khóa" : "mở khóa"
  } tài khoản này không?`;

  confirmModal = new bootstrap.Modal(document.getElementById("confirmModal"));
  confirmModal.show();
};

const confirmAction = async () => {
  try {
    const response =
      modalAction.value === "lock"
        ? await lockUser(selectedUserId.value)
        : await unlockUser(selectedUserId.value);
    console.log(response.status);
    if (
      response.code === 200 ||
      response.data.success === "success" ||
      response.data.status === 200
    ) {
      alert(
        modalAction.value === "lock"
          ? "Khóa tài khoản thành công"
          : "Mở khóa tài khoản thành công"
      );
      await getListAllUser();
      confirmModal.hide();
    } else {
      alert(
        modalAction.value === "lock"
          ? "Khóa tài khoản thất bại"
          : "Mở khóa tài khoản thất bại"
      );
    }
  } catch (error) {
    console.error(`Error ${modalAction.value}ing user:`, error);
    alert(
      `Lỗi khi ${modalAction.value === "lock" ? "khóa" : "mở khóa"} tài khoản`
    );
  }
};

onMounted(() => {
  getListAllUser();
});
</script>

<style scoped>
.table {
  width: 100%;
  margin-bottom: 1rem;
  color: #212529;
}

.table th,
.table td {
  padding: 0.75rem;
  vertical-align: middle;
  border: 1px solid #dee2e6;
}

.table thead th {
  background-color: #343a40;
  color: white;
}

.btn-sm {
  padding: 0.25rem 0.5rem;
  font-size: 0.875rem;
}
</style>
