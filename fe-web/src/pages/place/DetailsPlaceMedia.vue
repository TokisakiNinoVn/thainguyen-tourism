<template>
  <div class="relative min-h-screen">
    <!-- Main Content -->
    <div class="relative">
      <!-- Left Sidebar (Place Info) -->
      <div 
        :class="{
          'translate-x-0': showLeftMenu,
          '-translate-x-full': !showLeftMenu
        }" 
        class="fixed top-0 left-0 h-full w-80 bg-white shadow-xl z-40 transition-transform duration-300 overflow-y-auto"
      >
        <div class="p-4">
          <!-- Close/Open Button -->
          <button 
            @click="showLeftMenu = !showLeftMenu"
            class="absolute top-4 right-4 text-gray-600 hover:text-gray-800"
          >
            <svg v-if="showLeftMenu" class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
            <svg v-else class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>

          <!-- Place Info -->
          <div class="mt-8">
            <h5 class="text-xl font-semibold text-blue-600">Thông tin địa điểm</h5>
            <h6 class="text-2xl font-bold mt-4 mb-4">{{ place.name || 'Đang tải...' }}</h6>
            <img
              v-if="place.imageUrl"
              :src="instance.defaults.baseURL + place.imageUrl"
              alt="Ảnh đại diện"
              class="w-full max-w-md rounded-lg shadow-md mb-4 cursor-pointer"
            />
            <div v-html="place.description || ''" class="prose max-w-none mb-4"></div>
            <div class="grid grid-cols-1 gap-4">
              <p><strong>Vĩ độ:</strong> {{ place.latitude ?? 'N/A' }}</p>
              <p><strong>Kinh độ:</strong> {{ place.longitude ?? 'N/A' }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Right Sidebar (Reviews) -->
      <div 
        :class="{
          'translate-x-0': showRightMenu,
          'translate-x-full': !showRightMenu
        }" 
        class="fixed top-0 right-0 h-full w-80 bg-white shadow-xl z-40 transition-transform duration-300 overflow-y-auto"
      >
        <div class="p-4">
          <!-- Close/Open Button -->
          <button 
            @click="showRightMenu = !showRightMenu"
            class="absolute top-4 left-4 text-gray-600 hover:text-gray-800"
          >
            <svg v-if="showRightMenu" class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
            <svg v-else class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>

          <!-- Reviews -->
          <div class="mt-8">
            <h5 class="text-xl font-semibold text-blue-600">Đánh giá</h5>
            <button
              v-if="!showReviewForm"
              class="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition-colors mt-4"
              @click="showReviewForm = true"
            >
              Thêm đánh giá
            </button>
            <div v-if="showReviewForm" class="mb-4 mt-4">
              <div class="flex items-center mb-4">
                <span class="mr-2">Điểm đánh giá:</span>
                <div class="flex">
                  <button
                    v-for="star in 5"
                    :key="star"
                    @click="newReview.rating = star"
                    class="text-2xl"
                    :class="star <= newReview.rating ? 'text-yellow-400' : 'text-gray-300'"
                  >
                    ★
                  </button>
                </div>
              </div>
              <textarea
                v-model="newReview.comment"
                class="w-full p-2 border rounded-lg"
                rows="3"
                placeholder="Viết đánh giá của bạn..."
              ></textarea>
              <div class="mt-2 flex gap-2">
                <button
                  class="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition-colors"
                  @click="submitReview"
                >
                  Gửi đánh giá
                </button>
                <button
                  class="bg-gray-300 text-gray-700 px-4 py-2 rounded-lg hover:bg-gray-400 transition-colors"
                  @click="cancelReview"
                >
                  Hủy
                </button>
              </div>
            </div>
            <div class="mt-6">
              <div
                v-for="review in reviews"
                :key="review.id"
                class="border-b border-gray-200 py-4 last:border-b-0"
              >
                <div class="flex items-center mb-2">
                  <img
                    alt="Avatar"
                    class="w-10 h-10 rounded-full mr-3"
                    src="https://i.pinimg.com/736x/48/e1/02/48e102b6e1dcf19306de4cc1504b2205.jpg"
                  />
                  <span class="font-semibold text-lg">{{ review.displayName }}</span>
                  <div class="ml-3 flex">
                    <span
                      v-for="star in 5"
                      :key="star"
                      class="text-yellow-400"
                      :class="{ 'opacity-30': star > review.rating }"
                    >★</span>
                  </div>
                </div>
                <p class="text-gray-600">{{ review.content }}</p>
                <p class="text-sm text-gray-500 mt-1">{{ formatDate(review.createdAt) }}</p>
              </div>
              <p v-if="!reviews.length" class="text-gray-500">Chưa có đánh giá nào.</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Toggle Buttons for Sidebars -->
      <button
        @click="showLeftMenu = !showLeftMenu"
        class="fixed top-1/2 left-0 z-50 bg-blue-500 text-white p-2 rounded-r-lg transform -translate-y-1/2"
      >
        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
        </svg>
      </button>
      <button
        @click="showRightMenu = !showRightMenu"
        class="fixed top-1/2 right-0 z-50 bg-blue-500 text-white p-2 rounded-l-lg transform -translate-y-1/2"
      >
        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
        </svg>
      </button>

      <!-- Main Media Display -->
      <div class="relative w-full h-screen bg-black flex items-center justify-center">
        <!-- Media Content -->
        <div class="w-full h-full flex items-center justify-center">
          <img
            v-if="currentMedia && currentMedia.mediaType === 1"
            :src="instance.defaults.baseURL + currentMedia.mediaUrl"
            class="w-full h-full object-contain"
          />
          <div
            v-if="currentMedia && currentMedia.mediaType === 2"
            :id="'pannellum-viewer-' + currentMedia.id"
            class="w-full h-full"
          ></div>
          <video
            v-if="currentMedia && currentMedia.mediaType === 3"
            :src="instance.defaults.baseURL + currentMedia.mediaUrl"
            controls
            autoplay
            class="w-full h-full object-contain"
          ></video>
        </div>

        <!-- Control Bar -->
        <div class="absolute bottom-4 left-0 right-0 flex justify-center z-30">
          <div class="bg-gray-800 bg-opacity-75 rounded-lg p-4 flex items-center gap-4">
            <!-- Previous Button -->
            <button
              class="text-white hover:text-gray-300"
              @click="prevMedia"
              title="Media trước"
            >
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
              </svg>
            </button>
            <!-- Zoom In (360 only) -->
            <button
              class="text-white hover:text-gray-300"
              :disabled="currentMedia?.mediaType !== 2"
              @click="zoomIn"
              :title="currentMedia?.mediaType === 2 ? 'Phóng to' : 'Không khả dụng'"
              :class="{ 'opacity-50 cursor-not-allowed': currentMedia?.mediaType !== 2 }"
            >
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
              </svg>
            </button>
            <!-- Zoom Out (360 only) -->
            <button
              class="text-white hover:text-gray-300"
              :disabled="currentMedia?.mediaType !== 2"
              @click="zoomOut"
              :title="currentMedia?.mediaType === 2 ? 'Thu nhỏ' : 'Không khả dụng'"
              :class="{ 'opacity-50 cursor-not-allowed': currentMedia?.mediaType !== 2 }"
            >
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 12H4" />
              </svg>
            </button>
            <!-- Auto Rotate (360 only) -->
            <button
              class="text-white hover:text-gray-300"
              :disabled="currentMedia?.mediaType !== 2"
              @click="toggleAutoRotate"
              :title="currentMedia?.mediaType === 2 ? (isAutoRotating ? 'Tắt tự động xoay' : 'Bật tự động xoay') : 'Không khả dụng'"
              :class="{ 'opacity-50 cursor-not-allowed': currentMedia?.mediaType !== 2 }"
            >
              <svg
                class="w-6 h-6"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
                :class="{ 'text-yellow-400': isAutoRotating && currentMedia?.mediaType === 2 }"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M4 4v5h5m0 0l-3-3m3 3l3-3m6 6v5h-5m0 0l3 3m-3-3l-3 3"
                />
              </svg>
            </button>
            <!-- Audio Controls -->
            <button
              class="text-white hover:text-gray-300"
              @click="toggleAudioMute"
              :title="isMuted ? 'Bật âm thanh' : 'Tắt âm thanh'"
            >
              <svg
                v-if="isMuted"
                class="w-6 h-6"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M5.586 15H4a1 1 0 01-1-1v-4a1 1 0 011-1h1.586l4.707-4.707A1 1 0 0112 5v14a1 1 0 01-1.707.707L5.586 15zM17 9l4 4m0-4l-4 4"
                />
              </svg>
              <svg
                v-else
                class="w-6 h-6"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M15.536 8.464a5 5 0 010 7.072m2.828-9.9a9 9 0 010 12.728M5.586 15H4a1 1 0 01-1-1v-4a1 1 0 011-1h1.586l4.707-4.707A1 1 0 0112 5v14a1 1 0 01-1.707.707L5.586 15z"
                />
              </svg>
            </button>
            <button
              class="text-white hover:text-gray-300"
              @click="toggleAudioPlay"
              :title="isAudioPlaying ? 'Tạm dừng' : 'Phát'"
            >
              <svg
                v-if="isAudioPlaying"
                class="w-6 h-6"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M10 9v6m4-6v6m7-3a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              <svg
                v-else
                class="w-6 h-6"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M14.752 11.168l-3.197-2.132A1 1 0 0010 9.87v4.263a1 1 0 001.555.832l3.197-2.132a1 1 0 000-1.664z"
                />
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </button>
            <input
              type="range"
              min="0"
              max="1"
              step="0.01"
              v-model="audioVolume"
              class="w-24"
              @input="adjustVolume"
              title="Điều chỉnh âm lượng"
            />
            <!-- Next Button -->
            <button
              class="text-white hover:text-gray-300"
              @click="nextMedia"
              title="Media tiếp theo"
            >
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
              </svg>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, nextTick } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getPlaceByIdApi, getReviewPlaceByIdApi } from '@/services/modules/place.api';
import { createReviewApi } from '@/services/modules/review.api';
import { getListMediaPlaceApi } from '@/services/modules/mediaplace.api';
import instance from '@/services/axiosConfig';

// Get place ID from route
const route = useRoute();
const router = useRouter();
const placeId = route.params.id;

// Reactive state for place and media
const place = ref({});
const allMedia = ref([]);
const images = ref([]);
const images360 = ref([]);
const videos = ref([]);
const audios = ref([]);

// Reviews
const reviews = ref([]);
const showReviewForm = ref(false);
const newReview = ref({
  rating: 0,
  comment: ''
});

// Sidebar states
const showLeftMenu = ref(true);
const showRightMenu = ref(true);

// Media state
const currentMediaIndex = ref(0);
const currentMedia = ref(null);
const audioElement = ref(null);
const isAudioPlaying = ref(false);
const isMuted = ref(false);
const audioVolume = ref(0.5);
const isAutoRotating = ref(true);
const pannellumViewer = ref(null);

// format date function
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
  
// Fetch place details
const fetchPlaceDetails = async () => {
  try {
    const response = await getPlaceByIdApi(placeId);
    place.value = response.data.data || {};
  } catch (error) {
    console.error('Lỗi khi tải thông tin địa điểm:', error);
  }
};

// Fetch reviews
const fetchReviews = async () => {
  try {
    const response = await getReviewPlaceByIdApi(placeId);
    reviews.value = JSON.parse(response.data.data) || [];
  } catch (error) {
    console.error('Lỗi khi tải đánh giá:', error);
  }
};

// Submit review
const submitReview = async () => {
  if (!newReview.value.rating || !newReview.value.comment.trim()) {
    alert('Vui lòng chọn số sao và viết nhận xét!');
    return;
  }
  const isLogin = localStorage.getItem('isLoggedIn');
  if (isLogin === 'false') {
    alert('Vui lòng đăng nhập để gửi đánh giá!');
    router.push('/login');
    return;
  }

  try {
    const payload = {
      placeId: parseInt(placeId),
      rating: newReview.value.rating,
      reviewDescription: newReview.value.comment
    };
    
    const response = await createReviewApi(payload);
    if (response.status === 200) { 
      newReview.value = { rating: 0, comment: '' };
      showReviewForm.value = false;
      await fetchReviews();
      alert('Đánh giá của bạn đã được gửi thành công! Chúng tôi sẽ sớm duyệt nó.');
    }
  } catch (error) {
    console.error('Lỗi khi gửi đánh giá:', error);
    alert('Đã có lỗi xảy ra khi gửi đánh giá!');
  }
};

const cancelReview = () => {
  newReview.value = { rating: 0, comment: '' };
  showReviewForm.value = false;
};

// Fetch and categorize media
const fetchMediaList = async () => {
  try {
    const response = await getListMediaPlaceApi(placeId);
    const mediaData = JSON.parse(response.data.data) || [];
    allMedia.value = mediaData;
    images.value = mediaData.filter((item) => item.mediaType === 1);
    images360.value = mediaData.filter((item) => item.mediaType === 2);
    videos.value = mediaData.filter((item) => item.mediaType === 3);
    audios.value = mediaData.filter((item) => item.mediaType === 4);
    if (mediaData.length > 0) {
      currentMediaIndex.value = 0;
      currentMedia.value = mediaData[0];
      initializeAudio();
      initialize360Viewer();
    }
  } catch (error) {
    console.error('Lỗi khi tải danh sách media:', error);
  }
};

// Media Navigation Functions
const prevMedia = () => {
  if (allMedia.value.length > 0) {
    currentMediaIndex.value =
      (currentMediaIndex.value - 1 + allMedia.value.length) % allMedia.value.length;
    currentMedia.value = allMedia.value[currentMediaIndex.value];
    initialize360Viewer();
  }
};

const nextMedia = () => {
  if (allMedia.value.length > 0) {
    currentMediaIndex.value =
      (currentMediaIndex.value + 1) % allMedia.value.length;
    currentMedia.value = allMedia.value[currentMediaIndex.value];
    initialize360Viewer();
  }
};

// 360 Image Controls
const zoomIn = () => {
  if (pannellumViewer.value && currentMedia.value?.mediaType === 2) {
    const currentHfov = pannellumViewer.value.getHfov();
    pannellumViewer.value.setHfov(currentHfov - 10, 200); // Zoom in by reducing HFOV
  }
};

const zoomOut = () => {
  if (pannellumViewer.value && currentMedia.value?.mediaType === 2) {
    const currentHfov = pannellumViewer.value.getHfov();
    pannellumViewer.value.setHfov(currentHfov + 10, 200); // Zoom out by increasing HFOV
  }
};
const toggleAutoRotate = () => {
  if (pannellumViewer.value && currentMedia.value?.mediaType === 2) {
    isAutoRotating.value = !isAutoRotating.value;

    if (isAutoRotating.value) {
      pannellumViewer.value.startAutoRotate(-2);
    } else {
      pannellumViewer.value.stopAutoRotate();
    }
  }
};

const initializeAudio = () => {
  const audioMedia = audios.value[0];
  if (audioMedia) {
    audioElement.value = new Audio(instance.defaults.baseURL + audioMedia.mediaUrl);
    audioElement.value.volume = audioVolume.value;
    audioElement.value.loop = true;
    audioElement.value.play().then(() => {
      isAudioPlaying.value = true;
    }).catch((error) => {
      console.error('Lỗi khi phát audio:', error);
    });
  }
};

const toggleAudioPlay = () => {
  if (audioElement.value) {
    if (isAudioPlaying.value) {
      audioElement.value.pause();
      isAudioPlaying.value = false;
    } else {
      audioElement.value.play().then(() => {
        isAudioPlaying.value = true;
      }).catch((error) => {
        console.error('Lỗi khi phát audio:', error);
      });
    }
  }
};

const toggleAudioMute = () => {
  if (audioElement.value) {
    isMuted.value = !isMuted.value;
    audioElement.value.muted = isMuted.value;
  }
};

const adjustVolume = () => {
  if (audioElement.value) {
    audioElement.value.volume = audioVolume.value;
    isMuted.value = audioVolume.value === 0;
    audioElement.value.muted = isMuted.value;
  }
};

const initialize360Viewer = async () => {
  if (currentMedia.value && currentMedia.value.mediaType === 2) {
    const viewerId = `pannellum-viewer-${currentMedia.value.id}`;
    await nextTick(); // Wait for DOM to update
    const viewerElement = document.getElementById(viewerId);
    if (!viewerElement) {
      console.error(`Viewer element with ID ${viewerId} not found`);
      return;
    }

    // Clear existing viewer if present
    if (viewerElement.firstChild) {
      viewerElement.innerHTML = '';
    }

    try {
      const panoramaUrl = instance.defaults.baseURL + currentMedia.value.mediaUrl;
      const response = await fetch(panoramaUrl, { method: 'HEAD' });
      if (!response.ok) {
        console.error(`Image at ${panoramaUrl} is not accessible: ${response.status}`);
        return;
      }

      if (!window.pannellum) {
        console.error('Pannellum library is not loaded');
        return;
      }

      setTimeout(() => {
        try {
          pannellumViewer.value = window.pannellum.viewer(viewerId, {
            type: 'equirectangular',
            panorama: panoramaUrl,
            autoLoad: true,
            showZoomCtrl: false, // Hide default zoom controls
            showFullscreenCtrl: true,
            autoRotate: isAutoRotating.value ? -2 : false,
          });
        } catch (error) {
          console.error('Error initializing Pannellum viewer:', error);
        }
      }, 100);
    } catch (error) {
      console.error(`Failed to fetch image at:`, error);
    }
  } else {
    pannellumViewer.value = null;
  }
};

// Watch for changes in currentMedia to handle 360 viewer initialization
watch(currentMedia, () => {
  isAutoRotating.value = true; // Reset to auto-rotate for new 360 images
  initialize360Viewer();
});

// Load Pannellum assets and data
onMounted(() => {
  const existingLink = document.querySelector('link[href="https://cdn.jsdelivr.net/npm/pannellum@2.5.5/build/pannellum.css"]');
  if (!existingLink) {
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = 'https://cdn.jsdelivr.net/npm/pannellum@2.5.5/build/pannellum.css';
    document.head.appendChild(link);
  }

  const existingScript = document.querySelector('script[src="https://cdn.jsdelivr.net/npm/pannellum@2.5.5/build/pannellum.js"]');
  if (!existingScript) {
    const script = document.createElement('script');
    script.src = 'https://cdn.jsdelivr.net/npm/pannellum@2.5.5/build/pannellum.js';
    script.async = true;
    script.onload = () => console.log('Pannellum script loaded');
    script.onerror = () => console.error('Failed to load Pannellum script');
    document.head.appendChild(script);
  }

  // Load data
  fetchPlaceDetails();
  fetchReviews();
  fetchMediaList();
});
</script>