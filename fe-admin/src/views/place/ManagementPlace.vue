<template>
  <div class="py-4 container-fluid">
    <div class="row">
      <div class="col-12">
        <h3 class="text-center">Quản lý địa điểm</h3>
      </div>
    </div>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-header">
            <h5 class="mb-0">Danh sách địa điểm</h5>
          </div>
          <div class="card-body">
            <table class="table table-bordered table-hover">
              <thead class="thead-dark">
                <tr>
                  <th scope="col">Thumbnail</th>
                  <th scope="col">Tên</th>
                  <th scope="col">Mô tả</th>
                  <th scope="col">Kinh độ</th>
                  <th scope="col">Vĩ độ</th>
                  <th scope="col">Ngày tạo</th>
                  <th scope="col">Thao tác</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="place in placeList" :key="place.id">
                  <!-- <td>{{ place.id }}</td> -->
                  <td >{{ place.thumnail }}
                    <img v-if="place.thumnail !== undefined && place.thumnail !== null"
                      :src="instance.defaults.baseURL + place.thumnail"
                      alt="Thumbnail"
                      class="img-thumbnail"
                      style="width: 100px; height: 100px;"
                    />
                    <span v-else>
                      <img
                        src="https://i.pinimg.com/736x/3e/06/e4/3e06e4b2d23bc6084986ca2804954f3c.jpg"
                        alt="Placeholder"
                        class="img-thumbnail"
                        style="width: 100px;"
                      />
                    </span>
                  </td>
                  <td>{{ place.name }}</td>
                  <td>{{ place.description }}</td>
                  <td>{{ place.longitude }}</td>
                  <td>{{ place.latitude }}</td>
                  <td>{{ formatDate(place.createdAt) }}</td>
                  <td>
                    <button
                      class="btn btn-sm btn-primary me-2"
                      @click="goToEdit(place.id)"
                    >
                      Sửa
                    </button>
                    <button
                      class="btn btn-sm btn-danger me-2"
                      @click="confirmDelete(place.id)"
                    >
                      Xóa
                    </button>
                    <button
                      class="btn btn-sm btn-success"
                      @click="goToAddMedia(place.id)"
                    >
                      Thêm phương tiện
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal xác nhận xóa -->
    <div
      class="modal fade"
      id="deleteModal"
      tabindex="-1"
      aria-labelledby="deleteModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="deleteModalLabel">Xác nhận xóa</h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body">
            Bạn có chắc chắn muốn xóa địa điểm này không?
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
              class="btn btn-danger"
              @click="deletePlaceConfirmed"
            >
              Xóa
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { getListPlaceApi, deletePlaceApi } from '@/apis/modules/place.api';
import { useRouter } from 'vue-router';
import { ref, onMounted } from 'vue';
import * as bootstrap from 'bootstrap';
import instance from '@/apis/axiosConfig';

const placeList = ref([]);
const placeIdToDelete = ref(null);
const router = useRouter();

const getListPlace = async () => {
  try {
    const res = await getListPlaceApi();
    if (res && res.data) {
      placeList.value = res.data;
    }
  } catch (error) {
    console.error('Lỗi khi lấy danh sách địa điểm:', error);
  }
};

const formatDate = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
  });
};

const goToEdit = (placeId) => {
  router.push(`/places/edit/${placeId}`);
};

const goToAddMedia = (placeId) => {
  router.push(`/places/${placeId}/media/add`);
};

const confirmDelete = (placeId) => {
  placeIdToDelete.value = placeId;
  const modal = new bootstrap.Modal(document.getElementById('deleteModal'));
  modal.show();
};

const deletePlaceConfirmed = async () => {
  if (placeIdToDelete.value) {
    try {
      const res = await deletePlaceApi(placeIdToDelete.value);
      if (res && res.data) {
        await getListPlace();
        const modal = bootstrap.Modal.getInstance(
          document.getElementById('deleteModal')
        );
        modal.hide();
      }
    } catch (error) {
      console.error('Lỗi khi xóa địa điểm:', error);
    }
  }
};

onMounted(() => {
  getListPlace();
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