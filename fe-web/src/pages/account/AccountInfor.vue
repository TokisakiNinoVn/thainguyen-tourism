<template>
  <NavbarComponentV1 />
  <div
    class="relative min-h-screen bg-cover bg-center"
    style="
      background-image: url('https://toquoc.mediacdn.vn/Uploaded/minhkhanh/2018_08_19/chef/BINM4708_AOAV.jpg');
    "
  >
    <!-- Overlay for readability -->
    <div class="absolute inset-0 bg-black/50"></div>

    <!-- Profile Container -->
    <div
      class="relative mx-auto max-w-5xl p-6 md:p-10 mt-8 mb-12 bg-white/95 backdrop-blur-sm rounded-2xl shadow-2xl animate-fadeIn"
    >
      <h1
        class="text-4xl font-bold text-green-800 text-center mb-10 tracking-tight"
      >
        Hồ Sơ Cá Nhân
      </h1>

      <!-- Tab Navigation -->
      <div class="tabs flex border-b border-green-200 mb-8">
        <button
          v-for="tab in tabs"
          :key="tab.id"
          @click="currentTab = tab.id"
          :class="[
            'px-6 py-3 text-lg font-medium transition-all duration-300',
            currentTab === tab.id
              ? 'border-b-2 border-green-500 text-green-600'
              : 'text-gray-600 hover:text-green-600 hover:bg-green-50',
          ]"
        >
          {{ tab.name }}
        </button>
      </div>

      <!-- Tab Content -->
      <div
        class="tab-content bg-white/95 backdrop-blur-sm shadow-lg rounded-xl p-8 min-h-[400px]"
      >
        <!-- Profile Tab -->
        <div
          v-if="currentTab === 'profile'"
          class="profile-card flex flex-col items-center"
        >
          <img
            :src="userInfor.photoURL || defaultAvatar"
            alt="Avatar"
            class="avatar w-28 h-28 rounded-full object-cover border-4 border-green-500 mb-6 shadow-md transform hover:scale-105 transition duration-300"
          />
          <div class="info w-full text-gray-700 space-y-4">
            <p class="flex items-center text-lg">
              <strong class="w-40 text-green-800">Tên hiển thị:</strong>
              {{ userInfor.displayName || "Chưa có tên" }}
            </p>
            <p class="flex items-center text-lg">
              <strong class="w-40 text-green-800">Email:</strong>
              {{ userInfor.email }}
            </p>
            <p
              v-if="userInfor.isGoogleLogin === 1"
              class="flex items-center text-lg"
            >
              <strong class="w-40 text-green-800">Đăng nhập Google:</strong>
              <span :class="['status', 'yes']">Có</span>
            </p>
            <p
              v-if="userInfor.isGoogleLogin === 1"
              class="flex items-center text-lg"
            >
              <strong class="w-40 text-green-800">Liên kết tài khoản:</strong>
              <span :class="['status', userInfor.isLink ? 'yes' : 'no']">
                {{ userInfor.isLink ? "Đã liên kết" : "Chưa liên kết" }}
              </span>
            </p>
            <p class="flex items-center text-lg">
              <strong class="w-40 text-green-800">Token Chat:</strong>
              {{ userInfor.tokenChat }}
            </p>
          </div>

          <!-- Link Account Form -->
          <div
            v-if="userInfor.isGoogleLogin === 1 && userInfor.isLink !== 1"
            class="link-account-form w-full mt-8"
          >
            <h3 class="text-xl font-semibold text-green-800 mb-6">
              Liên Kết Tài Khoản
            </h3>
            <div class="space-y-4">
              <input
                v-model="password"
                type="password"
                placeholder="Nhập mật khẩu"
                class="w-full p-4 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-green-500 transition duration-200"
              />
              <input
                v-model="confirmPassword"
                type="password"
                placeholder="Nhập lại mật khẩu"
                class="w-full p-4 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-green-500 transition duration-200"
              />
              <button
                @click="linkAccount"
                class="w-full bg-green-500 text-white px-6 py-3 rounded-lg hover:bg-green-600 transition-all duration-300 transform hover:-translate-y-1"
              >
                Liên Kết
              </button>
            </div>
          </div>

          <!-- Update Display Name Form -->
          <div
            v-if="userInfor.isGoogleLogin !== 1"
            class="update-display-name w-full mt-8"
          >
            <h3 class="text-xl font-semibold text-green-800 mb-6">
              Cập Nhật Tên Hiển Thị
            </h3>
            <div class="space-y-4">
              <input
                v-model="newDisplayName"
                type="text"
                placeholder="Nhập tên hiển thị mới"
                class="w-full p-4 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-green-500 transition duration-200"
              />
              <button
                @click="updateDisplayName"
                class="w-full bg-green-500 text-white px-6 py-3 rounded-lg hover:bg-green-600 transition-all duration-300 transform hover:-translate-y-1"
              >
                Cập Nhật
              </button>
            </div>
          </div>
        </div>

        <!-- Posts Management Tab -->
        <div
          v-if="currentTab === 'posts'"
          class="posts-management min-h-[700px]"
        >
          <h3 class="text-xl font-semibold text-green-800 mb-6">
            Quản Lý Bài Đăng
          </h3>
          <div v-if="listBlog.length === 0" class="text-center py-8">
            <p class="text-gray-600 text-lg">
              Bạn chưa có bài đăng nào. Hãy tạo bài viết mới!
            </p>
            <router-link
              to="/create-blog"
              class="mt-4 inline-block bg-green-500 text-white px-6 py-3 rounded-lg hover:bg-green-600 transition-all duration-300"
            >
              Tạo Bài Viết
            </router-link>
          </div>
          <div class="grid gap-4">
            <div
              v-for="post in listBlog"
              :key="post.Id"
              class="flex items-center bg-white/95 backdrop-blur-sm p-4 rounded-lg shadow-md hover:shadow-xl transition-all duration-300 transform hover:-translate-y-1"
            >
              <img
                :src="getThumbnailUrl(post)"
                alt="Thumbnail"
                class="w-20 h-20 object-cover rounded-md mr-4"
              />
              <div class="flex-1">
                <h4 class="text-lg font-semibold text-gray-800 line-clamp-1">
                  {{ post.Title }}
                </h4>
                <p class="text-sm text-gray-600">Địa điểm: {{ post.Place }}</p>
                <p class="text-sm text-gray-600">
                  Trạng thái:
                  <span
                    :class="[
                      'font-semibold',
                      post.Status === 'draft'
                        ? 'text-yellow-600'
                        : 'text-green-600',
                    ]"
                  >
                    {{ post.Status === "draft" ? "Bản nháp" : "Đã đăng" }}
                  </span>
                </p>
                <p class="text-sm text-gray-500">
                  Ngày tạo: {{ formatDate(post.CreatedAt) }}
                </p>
              </div>
              <button
                @click="deletePost(post.Id)"
                class="ml-4 text-red-500 hover:text-red-700 transition-colors"
                title="Xóa bài đăng"
              >
                <svg
                  class="w-6 h-6"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M6 18L18 6M6 6l12 12"
                  />
                </svg>
              </button>
            </div>
          </div>
        </div>

        <!-- Favorites Management Tab -->
        <div
          v-if="currentTab === 'favorites'"
          class="favorites-management min-h-[700px]"
        >
          <h3 class="text-xl font-semibold text-green-800 mb-6">
            Địa Điểm Yêu Thích
          </h3>
          <div v-if="savePlaces.length === 0" class="text-center py-8">
            <p class="text-gray-600 text-lg">
              Bạn chưa có địa điểm yêu thích nào. Hãy khám phá Thái Nguyên!
            </p>
            <router-link
              to="/"
              class="mt-4 inline-block bg-green-500 text-white px-6 py-3 rounded-lg hover:bg-green-600 transition-all duration-300"
            >
              Khám Phá Ngay
            </router-link>
          </div>
          <div class="grid gap-4">
            <div
              v-for="place in savePlaces"
              :key="place.placeId"
              class="flex items-center bg-white/95 backdrop-blur-sm p-4 rounded-lg shadow-md hover:shadow-xl transition-all duration-300 transform hover:-translate-y-1"
            >
              <img
                :src="place.thumbnailUrl"
                alt="Thumbnail"
                class="w-20 h-20 object-cover rounded-md mr-4"
              />
              <div class="flex-1">
                <h4 class="text-lg font-semibold text-gray-800 line-clamp-1">
                  {{ place.placeName }}
                </h4>
                <p class="text-sm text-gray-600 line-clamp-2">
                  {{ getShortDescription(place.description) }}
                </p>
              </div>
              <button
                @click="deleteFavorite(place.placeId)"
                class="ml-4 text-red-500 hover:text-red-700 transition-colors"
                title="Xóa khỏi yêu thích"
              >
                <svg
                  class="w-6 h-6"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M6 18L18 6M6 6l12 12"
                  />
                </svg>
              </button>
            </div>
          </div>
        </div>
      </div>

      <button
        @click="confirmLogout"
        class="logout-btn mt-8 w-full bg-red-500 text-white px-6 py-3 rounded-lg hover:bg-red-600 transition-all duration-300 transform hover:-translate-y-1"
      >
        Đăng Xuất
      </button>
    </div>

    <FooterComponent />
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import NavbarComponentV1 from "@/components/NavbarComponentV1.vue";
import FooterComponent from "@/components/FooterComponent.vue";
import {
  getInforApi,
  linkAccountApi,
  updateDisplayNameApi,
} from "@/services/modules/user.api";
import {
  getSavePlaceApi,
  deleteUnSavePlaceApi,
} from "@/services/modules/save.api";
import { getBlogByUserApi, deleteBlogApi } from "@/services/modules/blog.api";
import instance from "@/services/axiosConfig";

const userInfor = ref({});
const password = ref("");
const confirmPassword = ref("");
const newDisplayName = ref("");
const defaultAvatar =
  "https://i.pinimg.com/736x/bd/61/ab/bd61ab78ce895f9e57744fe19040253c.jpg";
const defaultThumbnail =
  "https://i.pinimg.com/736x/bd/61/ab/bd61ab78ce895f9e57744fe19040253c.jpg";
const router = useRouter();
const currentTab = ref("profile");
const savePlaces = ref([]);
const listBlog = ref([]);

const tabs = [
  { id: "profile", name: "Thông Tin Cá Nhân" },
  { id: "posts", name: "Quản Lý Bài Đăng" },
  { id: "favorites", name: "Địa Điểm Yêu Thích" },
];

const getUserInfor = async () => {
  try {
    const response = await getInforApi();
    userInfor.value = response.data.data;
  } catch (error) {
    console.error("Lỗi khi lấy thông tin người dùng:", error);
  }
};

const getBlogByUser = async () => {
  try {
    const response = await getBlogByUserApi();
    listBlog.value = JSON.parse(response.data.data);
  } catch (error) {
    console.error("Lỗi khi lấy danh sách bài đăng:", error);
  }
};

const getSavePlaces = async () => {
  try {
    const response = await getSavePlaceApi();
    const parsedData = JSON.parse(response.data.data);

    savePlaces.value = parsedData.map((place) => ({
      ...place,
      thumbnailUrl:
        !place.thumbnailUrl ||
        (typeof place.thumbnailUrl === "object" &&
          Object.keys(place.thumbnailUrl).length === 0)
          ? defaultThumbnail
          : instance.defaults.baseURL + place.thumbnailUrl,
    }));
  } catch (error) {
    console.error("Lỗi khi lấy danh sách địa điểm đã lưu:", error);
  }
};

const deletePost = async (postId) => {
  const confirmed = window.confirm("Bạn có chắc chắn muốn xóa bài đăng này?");
  if (!confirmed) return;

  try {
    await deleteBlogApi(postId);
    listBlog.value = listBlog.value.filter((post) => post.Id !== postId);
    alert("Xóa bài đăng thành công!");
  } catch (error) {
    console.error("Lỗi khi xóa bài đăng:", error);
    alert("Xóa bài đăng thất bại. Vui lòng thử lại!");
  }
};

const deleteFavorite = async (placeId) => {
  const confirmed = window.confirm(
    "Bạn có chắc chắn muốn xóa địa điểm yêu thích này?"
  );
  if (!confirmed) return;

  try {
    await deleteUnSavePlaceApi(placeId);
    savePlaces.value = savePlaces.value.filter(
      (place) => place.placeId !== placeId
    );
    alert("Xóa địa điểm yêu thích thành công!");
  } catch (error) {
    console.error("Lỗi khi xóa địa điểm yêu thích:", error);
    alert("Xóa địa điểm yêu thích thất bại. Vui lòng thử lại!");
  }
};

const linkAccount = async () => {
  if (password.value !== confirmPassword.value) {
    alert("Mật khẩu không khớp!");
    return;
  }
  try {
    await linkAccountApi({
      password: password.value,
      confirmPassword: confirmPassword.value,
    });
    alert("Liên kết tài khoản thành công!");
    await getUserInfor();
    password.value = "";
    confirmPassword.value = "";
  } catch (error) {
    alert(
      "Liên kết thất bại: " +
        (error?.response?.data?.message || "Lỗi không xác định")
    );
  }
};

const updateDisplayName = async () => {
  if (!newDisplayName.value.trim()) {
    alert("Vui lòng nhập tên hiển thị.");
    return;
  }
  try {
    await updateDisplayNameApi({
      displayName: newDisplayName.value,
    });
    alert("Cập nhật tên hiển thị thành công!");
    await getUserInfor();
    newDisplayName.value = "";
  } catch (error) {
    alert(
      "Cập nhật thất bại: " +
        (error?.response?.data?.message || "Lỗi không xác định")
    );
  }
};

const confirmLogout = () => {
  const confirmed = window.confirm("Bạn có chắc chắn muốn đăng xuất?");
  if (confirmed) {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    localStorage.setItem("isLoggedIn", "false");
    router.push("/login");
  }
};

const getThumbnailUrl = (post) => {
  const thumbnailMedia = post.MediaList.find(
    (media) => media.imageFor === "thumbnail"
  );
  return thumbnailMedia
    ? instance.defaults.baseURL + thumbnailMedia.mediaUrl
    : defaultThumbnail;
};

const getShortDescription = (description) => {
  const tempEl = document.createElement("div");
  tempEl.innerHTML = description;
  const text = tempEl.textContent || tempEl.innerText || "";
  return text.length > 100 ? text.slice(0, 100) + "..." : text;
};

const formatDate = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleDateString("vi-VN", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
};

onMounted(() => {
  getUserInfor();
  getSavePlaces();
  getBlogByUser();
});
</script>

<style scoped>
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

.animate-fadeIn {
  animation: fadeIn 0.8s ease-out;
}

.status {
  display: inline-block;
  padding: 0.25rem 0.75rem;
  border-radius: 0.5rem;
  font-size: 0.875rem;
  font-weight: 600;
}

.status.yes {
  background-color: #d1fae5;
  color: #047857;
}

.status.no {
  background-color: #fee2e2;
  color: #b91c1c;
}

.line-clamp-1 {
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

@media (max-width: 640px) {
  .p-10 {
    padding: 1.5rem;
  }
  .text-4xl {
    font-size: 2rem;
  }
  .w-40 {
    width: 8rem;
  }
}
</style>
