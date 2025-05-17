<template>
  <div
    class="main-blog__content fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 transition-opacity duration-300"
  >
    <div
      class="content-detais__blogs bg-gray-50 w-[70vw] h-[90vh] rounded-xl shadow-2xl flex flex-col relative overflow-hidden"
    >
      <!-- Close Button -->
      <button
        class="absolute top-4 right-4 text-gray-600 hover:text-gray-800 p-2 rounded-full bg-white bg-opacity-80 hover:bg-opacity-100 transition-all"
        @click="$emit('close')"
        aria-label="Đóng popup"
      >
        <i class="fa-solid fa-times text-xl"></i>
      </button>

      <!-- Loading State -->
      <div v-if="isLoading" class="flex items-center justify-center h-full">
        <div class="flex items-center space-x-2">
          <div
            class="w-6 h-6 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
          <span class="text-lg font-semibold text-gray-600">Đang tải...</span>
        </div>
      </div>

      <!-- Blog Content -->
      <div v-else-if="blogDetails" class="flex flex-1 overflow-hidden">
        <!-- Image Viewer -->
        <div
          class="w-2/3 bg-gray-900 flex items-center justify-center relative overflow-hidden"
        >
          <button
            class="absolute left-6 text-white text-lg bg-gray-800 bg-opacity-70 p-2.5 rounded-full hover:bg-opacity-90 transition-transform transform hover:scale-105 z-10"
            @click="changeImage(-1)"
            :disabled="currentImageIndex === 0"
          >
            <i class="fa-solid fa-chevron-left"></i>
          </button>
          <div class="w-full h-full flex items-center justify-center">
            <img
              :src="currentImage"
              class="max-w-full max-h-[80%] object-contain rounded-xl shadow-2xl transition-all duration-300 hover:shadow-[0_0_20px_rgba(255,255,255,0.2)]"
              alt="Blog Image"
              @error="handleImageError"
            />
          </div>
          <button
            class="absolute right-6 text-white text-lg bg-gray-800 bg-opacity-70 p-2.5 rounded-full hover:bg-opacity-90 transition-transform transform hover:scale-105 z-10"
            @click="changeImage(1)"
            :disabled="currentImageIndex === mediaList.length - 1"
          >
            <i class="fa-solid fa-chevron-right"></i>
          </button>
          <!-- Image Counter -->
          <div
            class="absolute bottom-4 text-white bg-gray-800 bg-opacity-70 px-3 py-1 rounded-full text-sm"
          >
            {{ currentImageIndex + 1 }} / {{ mediaList.length || 1 }}
          </div>
        </div>

        <!-- Blog Details -->
        <div class="w-1/3 bg-white flex flex-col min-w-0">
          <!-- Scrollable Content -->
          <div class="flex-1 overflow-y-auto p-3 scrollbar-custom">
            <!-- Post Info -->
            <div class="mb-2">
              <div class="flex items-center mb-2">
                <img
                  :src="blogDetails.authorPhoto || defaultAvatar"
                  class="w-12 h-12 rounded-full mr-3 object-cover border-2 border-gray-200"
                  alt="User Avatar"
                />
                <div>
                  <span class="font-semibold text-gray-800 text-base">{{
                    blogDetails.authorName || "Ẩn danh"
                  }}</span>
                  <p class="text-xs text-gray-500">
                    {{ toLocalDateTime(blogDetails.createdAt) }}
                  </p>
                </div>
              </div>
              <div class="flex items-center text-green-600 text-sm">
                <i class="fa-solid fa-location-dot mr-2"></i>
                <span class="font-medium">{{
                  blogDetails.place || "Không xác định"
                }}</span>
              </div>
              <div class="mt-2 text-sm text-gray-600">
                <span class="font-medium">Trạng thái: </span>
                <span
                  :class="
                    blogDetails.status === 'draft'
                      ? 'text-yellow-600'
                      : 'text-green-600'
                  "
                >
                  {{ blogDetails.status === "draft" ? "Bản nháp" : "Đã đăng" }}
                </span>
              </div>
            </div>

            <!-- Post Content -->
            <div class="mb-6">
              <h2 class="text-xl font-bold text-gray-900 mb-4 leading-tight">
                {{ blogDetails.title }}
              </h2>
              <div
                class="prose prose-sm text-gray-700 leading-relaxed text-left"
                v-html="blogDetails.content"
              ></div>
            </div>
          </div>
        </div>
      </div>

      <!-- Error State -->
      <div v-else class="flex items-center justify-center h-full">
        <div class="text-center">
          <h3 class="text-lg font-semibold text-gray-600">
            Không tìm thấy bài viết
          </h3>
          <button
            class="mt-4 py-2 px-4 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
            @click="$emit('close')"
          >
            Đóng
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, computed, defineProps } from "vue";
import { getDetailBlogByIdApi } from "@/apis/modules/blog.api";
import instance from "@/apis/axiosConfig";

const props = defineProps({
  id: {
    type: [String, Number],
    required: true,
  },
});

const blogDetails = ref({});
const mediaList = ref([]);
const isLoading = ref(true);
const currentImageIndex = ref(0);

const defaultAvatar =
  "https://i.pinimg.com/736x/d9/c5/d9/d9c5d921d56bca25b5b8f6f9dd545cb0.jpg";
const defaultImage =
  "https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg";

const currentImage = computed(() => {
  return mediaList.value.length > 0
    ? `${instance.defaults.baseURL}${mediaList.value[currentImageIndex.value]?.mediaUrl || defaultImage}`
    : defaultImage;
});

const toLocalDateTime = (date) => {
  return new Date(date).toLocaleString("vi-VN", {
    year: "numeric",
    month: "long",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
};

const getBlogDetails = async (blogId) => {
  try {
    const response = await getDetailBlogByIdApi(blogId);
    const data = response.data;
    blogDetails.value = typeof data === "string" ? JSON.parse(data) : data;
    mediaList.value = blogDetails.value.mediaList || [];
  } catch (error) {
    console.error("Error fetching blog details:", error);
    blogDetails.value = null;
  } finally {
    isLoading.value = false;
  }
};

const changeImage = (direction) => {
  const newIndex = currentImageIndex.value + direction;
  if (newIndex >= 0 && newIndex < mediaList.value.length) {
    currentImageIndex.value = newIndex;
  }
};

const handleImageError = (event) => {
  event.target.src = defaultImage;
};

onMounted(() => {
  getBlogDetails(props.id);
});
</script>

<style>
@import "https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css";
@import "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css";

.main-blog__content {
  animation: fadeIn 0.3s ease-in-out;
  z-index: 9999;
}

.content-detais__blogs {
  width: 80vw;
  max-width: 1100px;
  height: 80vh;
  max-height: 90vh;
  overflow: hidden;
  border-radius: 1rem;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

.scrollbar-custom {
  scrollbar-width: thin;
  scrollbar-color: #6b7280 #f3f4f6;
}

.scrollbar-custom::-webkit-scrollbar {
  width: 6px;
}

.scrollbar-custom::-webkit-scrollbar-track {
  background: #f3f4f6;
  border-radius: 3px;
}

.scrollbar-custom::-webkit-scrollbar-thumb {
  background: #6b7280;
  border-radius: 3px;
}

.scrollbar-custom::-webkit-scrollbar-thumb:hover {
  background: #4b5563;
}

.prose img {
  max-width: 100%;
  height: auto;
  border-radius: 8px;
  margin: 0.75rem 0;
}

.prose p {
  margin-bottom: 0.75rem;
}

.prose h3 {
  margin-top: 1rem;
  margin-bottom: 0.5rem;
}
</style>
