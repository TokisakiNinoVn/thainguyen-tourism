<template>
  <div class="main-blog__content">
    <div class="min-h-screen bg-gray-100 flex flex-col">
      <!-- Loading State -->
      <div v-if="isLoading" class="flex items-center justify-center min-h-screen">
        <div class="text-xl font-semibold text-gray-600">Đang tải...</div>
      </div>

      <!-- Blog Content -->
      <div v-else-if="blogDetails" class="flex flex-1">
        <!-- Image Viewer -->
        <div class="w-2/3 bg-black relative flex items-center justify-center">
          <i
            class="fa-solid fa-chevron-left absolute left-4 text-white text-2xl bg-gray-800 bg-opacity-50 p-2 rounded-full cursor-pointer hover:bg-opacity-75 transition"
            @click="changeImage(-1)"
          ></i>
          <img
            :src="currentImage"
            class="max-h-screen w-auto object-contain rounded-lg"
            alt="Blog Image"
          />
          <i
            class="fa-solid fa-chevron-right absolute right-4 text-white text-2xl bg-gray-800 bg-opacity-50 p-2 rounded-full cursor-pointer hover:bg-opacity-75 transition"
            @click="changeImage(1)"
          ></i>
        </div>

        <!-- Blog Details -->
        <div class="w-1/3 flex flex-col bg-white shadow-lg">
          <!-- Scrollable Content -->
          <div class="flex-1 overflow-y-auto p-6 scrollbar-custom">
            <!-- Post Info -->
            <div class="mb-6">
              <div class="flex items-center mb-4">
                <img
                  :src="
                    blogDetails.authorPhoto ||
                    'https://i.pinimg.com/736x/d9/c5/d9/d9c5d921d56bca25b5b8f6f9dd545cb0.jpg'
                  "
                  class="w-12 h-12 rounded-full mr-3 object-cover"
                  alt="User Avatar"
                />
                <div>
                  <span class="font-semibold text-gray-800">{{
                    blogDetails.authorName || "Bí khách ẩn danh"
                  }}</span>
                  <p class="text-sm text-gray-500">{{
                    toLocalDateTime(blogDetails.createdAt)
                  }}</p>
                </div>
              </div>
              <div class="text-green-500">
                <i class="fa-solid fa-location-dot mr-1"></i>
                {{ blogDetails.place }}
              </div>
            </div>

            <!-- Post Content -->
            <div class="mb-6">
              <h2 class="text-2xl font-bold text-gray-800 mb-4">{{ blogDetails.title }}</h2>
              <div class="prose text-gray-700 leading-relaxed text-left">
                <div
                  :class="{ 'max-h-40 overflow-hidden': !isContentExpanded }"
                  class="transition-all duration-300"
                  ref="contentContainer"
                  v-html="blogDetails.content"
                ></div>
                <button
                  v-if="isContentTruncated"
                  @click="toggleContent"
                  class="mt-2 text-blue-600 hover:text-blue-800 font-medium transition"
                >
                  {{ isContentExpanded ? "Ẩn bớt nội dung" : "Xem thêm nội dung" }}
                </button>
              </div>
            </div>

            <!-- Comments Section -->
            <div>
              <h3 class="text-lg font-semibold text-gray-800 mb-4">Bình luận</h3>
              <div v-if="comments.length === 0" class="text-gray-500 mb-4">
                Chưa có bình luận nào.
              </div>
              <div v-for="(comment, index) in comments" :key="index" class="mb-4">
                <div class="flex items-start">
                  <img
                    :src="comment.userAvatar"
                    class="w-10 h-10 rounded-full mr-3"
                    alt="Comment Avatar"
                  />
                  <div>
                    <span class="font-bold text-gray-800">{{ comment.userName }}</span>
                    <p class="text-gray-600 mt-1 text-left">{{ comment.commentContent }}</p>
                    <p class="text-sm text-gray-400 mt-1 text-left">{{
                      toLocalDate(comment.createdAt)
                    }}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Comment Input (Fixed) -->
          <div class="p-6 border-t border-gray-200">
            <textarea
              v-model="newComment.content"
              placeholder="Viết bình luận của bạn..."
              rows="4"
              class="w-full p-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
            ></textarea>
            <button
              @click.prevent="submitComment"
              class="mt-2 bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition flex items-center"
            >
              <i class="fa-solid fa-paper-plane mr-2"></i> Gửi
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, computed, nextTick } from "vue";
import { useRoute } from "vue-router";
import { getDetailBlogByIdApi, postCreatCommentBlogApi, getCommentBlogByIdApi } from "@/services/modules/blog.api";
import instance from "@/services/axiosConfig";

// List of random avatars
const avatarList = [
  'https://i.pinimg.com/736x/18/6a/8d/186a8df5cfc29a2b2be8e6aaaecdabad.jpg',
  'https://i.pinimg.com/736x/fc/2f/8f/fc2f8f5dd3831e402cc87b12ca691246.jpg',
  'https://i.pinimg.com/736x/d4/ca/e0/d4cae0d4753a3e27c4bb905619272c91.jpg',
  'https://i.pinimg.com/736x/08/1b/87/081b8762185ff233b3f3a1fa0e60203d.jpg',
  'https://i.pinimg.com/736x/f2/92/f8/f292f8cb6ddacd30240ad6e210ede530.jpg'
];

const route = useRoute();
const blogId = route.params.id;

const blogDetails = ref({});
const mediaList = ref([]);
const comments = ref([]);
const isLoading = ref(true);
const newComment = ref({ content: "" });
const currentImageIndex = ref(0);
const isContentExpanded = ref(false);
const isContentTruncated = ref(false);

const currentImage = computed(() => {
  return mediaList.value.length > 0
    ? `${instance.defaults.baseURL}${
        mediaList.value[currentImageIndex.value]?.mediaUrl ||
        "https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg"
      }`
    : "https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg";
});

// Function to generate random name and avatar
const generateRandomUser = () => {
  const randomNumber = Math.floor(Math.random() * 1000) + 1;
  const randomAvatar = avatarList[Math.floor(Math.random() * avatarList.length)];
  return {
    name: `Vị lãng khách ẩn danh ${randomNumber}`,
    avatar: randomAvatar
  };
};

const toLocalDateTime = (date) => {
  return new Date(date).toLocaleString("vi-VN", {
    year: "numeric",
    month: "long",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
};

const toLocalDate = (date) => {
  return new Date(date).toLocaleDateString("vi-VN", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });
};

const getBlogDetails = async () => {
  try {
    const response = await getDetailBlogByIdApi(blogId);
    const listComment = await getCommentBlogByIdApi(blogId);
    const data = response.data.data;
    blogDetails.value = data;
    mediaList.value = data.mediaList || [];
    const stringData = listComment.data.data || [];
    
    // Process comments to add random names and avatars if not present
    comments.value = JSON.parse(stringData).map(comment => ({
      ...comment,
      userName: comment.userName || generateRandomUser().name,
      userAvatar: comment.userAvatar || generateRandomUser().avatar
    }));
  } catch (error) {
    console.error("Error fetching blog details:", error);
  } finally {
    isLoading.value = false;
    await nextTick();
    checkContentOverflow();
  }
};

const checkContentOverflow = () => {
  const contentContainer = document.querySelector(".prose > div");
  if (contentContainer) {
    isContentTruncated.value = contentContainer.scrollHeight > contentContainer.clientHeight;
  }
};

const changeImage = (direction) => {
  const newIndex = currentImageIndex.value + direction;
  if (newIndex >= 0 && newIndex < mediaList.value.length) {
    currentImageIndex.value = newIndex;
  }
};

const toggleContent = async () => {
  isContentExpanded.value = !isContentExpanded.value;
  await nextTick();
  checkContentOverflow();
};

const submitComment = async () => {
  if (newComment.value.content.trim()) {
    const comment = {
      blogId: blogId,
      commentContent: newComment.value.content,
    };
    try {
      await postCreatCommentBlogApi(comment);
      const { name, avatar } = generateRandomUser();
      comments.value.push({
        ...comment,
        userName: name,
        userAvatar: avatar,
        createdAt: new Date().toISOString(),
      });
      newComment.value.content = "";
    } catch (error) {
      console.error("Error submitting comment:", error);
    }
  }
};

onMounted(() => {
  getBlogDetails();
});
</script>

<style>
@import "https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css";
@import "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css";

.main-blog__content {
  width: 100vw;
  height: 100vh;
  overflow: hidden;
}

.w-1\/3 {
  height: 100vh;
  display: flex;
  flex-direction: column;
}

.scrollbar-custom {
  scrollbar-width: thin;
  scrollbar-color: #888 #f1f1f1;
}

.scrollbar-custom::-webkit-scrollbar {
  width: 8px;
}

.scrollbar-custom::-webkit-scrollbar-track {
  background: #f1f1f1;
}

.scrollbar-custom::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 4px;
}

.scrollbar-custom::-webkit-scrollbar-thumb:hover {
  background: #555;
}

.prose img {
  max-width: 100%;
  height: auto;
  border-radius: 8px;
}
</style>