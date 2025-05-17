<template>
  <div class="py-4 container-fluid">
    <div class="row">
      <div class="col-12">
        <h3 class="text-center">Quản lý bài viết</h3>
      </div>
    </div>
    <div class="row mb-3">
      <div class="col-12">
        <div class="card">
          <div
            class="card-header d-flex justify-content-between align-items-center flex-wrap"
          >
            <h5 class="mb-0">Danh sách bài viết</h5>
            <div class="d-flex align-items-center gap-3">
              <div class="search-box">
                <input
                  type="text"
                  class="form-control"
                  placeholder="Tìm kiếm theo tiêu đề..."
                  v-model="searchQuery"
                />
              </div>
              <div class="filter-buttons">
                <button
                  class="btn btn-outline-primary me-2"
                  :class="{ active: filterStatus === 'draft' }"
                  @click="filterStatus = 'draft'"
                >
                  Chưa duyệt
                </button>
                <button
                  class="btn btn-outline-primary"
                  :class="{ active: filterStatus === 'approved' }"
                  @click="filterStatus = 'approved'"
                >
                  Đã duyệt
                </button>
              </div>
            </div>
          </div>
          <div class="card-body">
            <table class="table table-bordered table-hover">
              <thead class="thead-dark">
                <tr>
                  <th scope="col">Thumbnail</th>
                  <th scope="col">Tiêu đề</th>
                  <th scope="col">Địa điểm</th>
                  <th scope="col">Trạng thái</th>
                  <th scope="col">Ngày tạo</th>
                  <th scope="col">Thao tác</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="blog in filteredBlogs" :key="blog.Id">
                  <td>
                    <img
                      v-if="blog.MediaList[0]?.mediaUrl"
                      :src="
                        instance.defaults.baseURL + blog.MediaList[0].mediaUrl
                      "
                      alt="Thumbnail"
                      style="width: 100px; height: auto"
                      @error="handleImageError"
                    />
                  </td>
                  <td class="text-truncate" :title="blog.Title">
                    {{ blog.Title }}
                  </td>
                  <td class="text-truncate" :title="blog.Place">
                    {{ blog.Place }}
                  </td>
                  <td>
                    <span
                      :class="
                        blog.Status === 'draft'
                          ? 'badge bg-warning'
                          : 'badge bg-success'
                      "
                    >
                      {{ blog.Status === "draft" ? "Chưa duyệt" : "Đã duyệt" }}
                    </span>
                  </td>
                  <td>{{ formatDate(blog.CreatedAt) }}</td>
                  <td>
                    <button
                      class="btn btn-sm btn-info me-1"
                      @click="openBlogDetail(blog.Id)"
                    >
                      Xem
                    </button>
                    <button
                      v-if="blog.Status === 'draft'"
                      class="btn btn-sm btn-success me-1"
                      @click="approvePost(blog.Id)"
                    >
                      Phê duyệt
                    </button>
                    <button
                      class="btn btn-sm btn-danger"
                      @click="confirmDelete(blog.Id)"
                    >
                      Xóa
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
            Bạn có chắc chắn muốn xóa bài viết này không?
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

    <!-- Popup chi tiết blog -->
    <DetailsBlog
      v-if="showBlogDetail"
      :id="selectedBlogId"
      @close="closeBlogDetail"
    />
  </div>
</template>

<script setup>
import {
  getListBlogApi,
  deleteBlogApi,
  putApproveBlogApi,
} from "@/apis/modules/blog.api";
import { ref, onMounted, computed } from "vue";
import * as bootstrap from "bootstrap";
import instance from "@/apis/axiosConfig";
import DetailsBlog from "../blog/DetailsBlog.vue";

const blogList = ref([]);
const placeIdToDelete = ref(null);
const showBlogDetail = ref(false);
const selectedBlogId = ref(null);
const filterStatus = ref("draft");
const searchQuery = ref("");

const defaultImage =
  "https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg";

const filteredBlogs = computed(() => {
  return blogList.value.filter((blog) => {
    const matchesStatus =
      filterStatus.value === "all" || blog.Status === filterStatus.value;
    const matchesSearch = blog.Title.toLowerCase().includes(
      searchQuery.value.toLowerCase()
    );
    return matchesStatus && matchesSearch;
  });
});

const getListBlog = async () => {
  try {
    const res = await getListBlogApi();
    if (res && res.data) {
      blogList.value =
        typeof res.data === "string" ? JSON.parse(res.data) : res.data;
    }
  } catch (error) {
    console.error("Lỗi khi lấy danh sách bài viết:", error);
  }
};

const confirmDelete = (id) => {
  placeIdToDelete.value = id;
  const modal = new bootstrap.Modal(document.getElementById("deleteModal"));
  modal.show();
};

const deletePlaceConfirmed = async () => {
  if (placeIdToDelete.value) {
    try {
      await deleteBlogApi(placeIdToDelete.value);
      await getListBlog();
      const modal = bootstrap.Modal.getInstance(
        document.getElementById("deleteModal")
      );
      modal.hide();
    } catch (error) {
      console.error("Lỗi khi xóa bài viết:", error);
    }
  }
};

const approvePost = async (id) => {
  try {
    await putApproveBlogApi(id);
    alert("Bài viết đã được phê duyệt!");
    await getListBlog();
  } catch (error) {
    console.error("Lỗi khi phê duyệt bài viết:", error);
  }
};

const openBlogDetail = (id) => {
  console.log("Blog ID:", id);
  selectedBlogId.value = id;
  showBlogDetail.value = true;
};

const closeBlogDetail = () => {
  showBlogDetail.value = false;
  selectedBlogId.value = null;
};

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString("vi-VN", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
};

const handleImageError = (event) => {
  event.target.src = defaultImage;
};

onMounted(() => {
  getListBlog();
});
</script>

<style scoped>
.table {
  width: 100%;
  margin-bottom: 1rem;
  color: #212529;
  border-collapse: collapse;
}

.table th,
.table td {
  padding: 0.75rem;
  vertical-align: middle;
  border: 2px solid #dee2e6;
}

.table thead th {
  background-color: #343a40;
  color: white;
  border-color: #454d55;
}

.btn-sm {
  padding: 0.25rem 0.5rem;
  font-size: 0.875rem;
}

img {
  object-fit: cover;
}

.filter-buttons .btn {
  min-width: 120px;
}

.filter-buttons .btn.active {
  background-color: #007bff;
  color: white;
  border-color: #007bff;
}

.badge {
  padding: 0.35em 0.65em;
  font-size: 0.75em;
  font-weight: 700;
  line-height: 1;
  text-align: center;
  white-space: nowrap;
  vertical-align: baseline;
  border-radius: 0.25rem;
}

.text-truncate {
  max-width: 200px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.search-box {
  width: 250px;
}

.search-box .form-control {
  height: 38px;
}
</style>
