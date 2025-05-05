<template>
    <div class="py-4 container-fluid">
      <div class="row">
        <div class="col-12">
          <h3 class="text-center white">Quản lý đánh giá</h3>
        </div>
      </div>
      <div class="row">
        <div class="col-12">
          <div class="card mb-3">
            <div class="card-body">
              <div class="row">
                <div class="col-md-6">
                  <label for="statusFilter" class="form-label">Lọc theo trạng thái:</label>
                  <select
                    id="statusFilter"
                    v-model="statusFilter"
                    class="form-select"
                    @change="getListReview"
                  >
                    <option value="">Tất cả</option>
                    <option value="0">Chưa duyệt</option>
                    <option value="1">Đã duyệt</option>
                  </select>
                </div>
              </div>
            </div>
          </div>
          <div class="card">
            <div class="card-header">
              <h5 class="mb-0">Danh sách đánh giá</h5>
            </div>
            <div class="card-body">
              <table class="table table-bordered table-hover">
                <thead class="thead-dark">
                  <tr>
                    <th scope="col">STT</th>
                    <th scope="col">Nội dung</th>
                    <!-- <th scope="col">Người dùng</th>
                    <th scope="col">Địa điểm</th> -->
                    <th scope="col">Số sao</th>
                    <th scope="col">Ngày tạo</th>
                    <th scope="col">Trạng thái</th>
                    <th scope="col">Hành động</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(review, index) in filteredReviews" :key="review.id">
                    <td>{{ index + 1 }}</td>
                    <td>{{ review.reviewDescription }}</td>
                    <!-- <td>{{ review.userId || 'N/A' }}</td>
                    <td>{{ review.placeId|| 'N/A' }}</td> -->
                    <td>{{ review.rating }}</td>
                    <td>{{ formatDate(review.createdAt) }}</td>
                    <td>
                      <span
                        :class="{
                          'badge bg-warning': review.status === 0,
                          'badge bg-success': review.status === 1,
                        }"
                      >
                        {{ review.status === 0 ? 'Chưa duyệt' : 'Đã duyệt' }}
                      </span>
                    </td>
                    <td>
                      <button
                        v-if="review.status === 0"
                        class="btn btn-success btn-sm me-2"
                        @click="confirmApprove(review.id)"
                      >
                        Duyệt
                      </button>
                      <button
                        class="btn btn-danger btn-sm"
                        @click="confirmDelete(review.id)"
                      >
                        Xóa
                      </button>
                    </td>
                  </tr>
                  <tr v-if="filteredReviews.length === 0">
                    <td colspan="6" class="text-center">Không có đánh giá nào phù hợp.</td>
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
              Bạn có chắc chắn muốn xóa đánh giá này không?
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
                @click="deleteReviewConfirmed"
              >
                Xóa
              </button>
            </div>
          </div>
        </div>
      </div>
  
      <!-- Modal xác nhận duyệt -->
      <div
        class="modal fade"
        id="approveModal"
        tabindex="-1"
        aria-labelledby="approveModalLabel"
        aria-hidden="true"
      >
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="approveModalLabel">Xác nhận duyệt</h5>
              <button
                type="button"
                class="btn-close"
                data-bs-dismiss="modal"
                aria-label="Close"
              ></button>
            </div>
            <div class="modal-body">
              Bạn có chắc chắn muốn duyệt đánh giá này không?
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
                class="btn btn-success"
                @click="approveReviewConfirmed"
              >
                Duyệt
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed, onMounted } from 'vue';
  import * as bootstrap from 'bootstrap';
  import {
    deleteReviewApi,
    getAllReviewApi,
    postApproveReviewApi,
  } from '@/apis/modules/review.api';
  
  const reviewList = ref([]);
  const statusFilter = ref('');
  const placeIdToDelete = ref(null);
  const placeIdToApprove = ref(null);
  
  const getListReview = async () => {
    try {
      const res = await getAllReviewApi();
      if (res && res.data) {
        reviewList.value = JSON.parse(res.data);
      }
    } catch (error) {
      console.error('Lỗi khi lấy danh sách đánh giá:', error);
    }
  };
  
  const filteredReviews = computed(() => {
    if (statusFilter.value === '') return reviewList.value;
    return reviewList.value.filter(
      (r) => String(r.status) === String(statusFilter.value)
    );
  });
  
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
  
  const confirmDelete = (placeId) => {
    placeIdToDelete.value = placeId;
    const modal = new bootstrap.Modal(document.getElementById('deleteModal'));
    modal.show();
  };
  
  const deleteReviewConfirmed = async () => {
    if (placeIdToDelete.value) {
      try {
        const res = await deleteReviewApi(placeIdToDelete.value);
        if (res && res.data) {
          await getListReview();
          const modal = bootstrap.Modal.getInstance(
            document.getElementById('deleteModal')
          );
          modal.hide();
        }
      } catch (error) {
        console.error('Lỗi khi xóa đánh giá:', error);
      }
    }
  };
  
  const confirmApprove = (placeId) => {
    placeIdToApprove.value = placeId;
    const modal = new bootstrap.Modal(document.getElementById('approveModal'));
    modal.show();
  };
  
  const approveReviewConfirmed = async () => {
    if (placeIdToApprove.value) {
      try {
        const res = await postApproveReviewApi(placeIdToApprove.value);
        if (res && res.data) {
          await getListReview();
          const modal = bootstrap.Modal.getInstance(
            document.getElementById('approveModal')
          );
          modal.hide();
        }
      } catch (error) {
        console.error('Lỗi khi duyệt đánh giá:', error);
      }
    }
  };
  
  onMounted(() => {
    getListReview();
  });
  </script>
  