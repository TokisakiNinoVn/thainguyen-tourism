<template>
  <NavbarComponentV1 />

  <!-- Main Container -->
  <div class="container mx-auto px-4 py-8">

    <!-- Place Info Card -->
    <div class="card bg-white shadow-xl rounded-lg overflow-hidden">
      <div class="card-header bg-gradient-to-r from-blue-500 to-blue-600 text-white p-4">
        <h5 class="text-xl font-semibold">Thông tin địa điểm</h5>
      </div>
      <div class="card-body p-6">
        <h6 class="text-2xl font-bold mb-4">{{ place.name || 'Đang tải...' }}</h6>
        <img
          v-if="place.imageUrl"
          :src="instance.defaults.baseURL + place.imageUrl"
          alt="Ảnh đại diện"
          class="w-full max-w-md rounded-lg shadow-md mb-4 cursor-pointer"
          @click="openFullScreen(instance.defaults.baseURL + place.imageUrl, 'image')"
        />
        <div v-html="place.description || ''" class="prose max-w-none mb-4"></div>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <p><strong>Vĩ độ:</strong> {{ place.latitude ?? 'N/A' }}</p>
          <p><strong>Kinh độ:</strong> {{ place.longitude ?? 'N/A' }}</p>
        </div>
      </div>
    </div>

    <!-- Media Button -->
    <div class="mt-8">
      <button
        class="bg-blue-500 text-white px-6 py-3 rounded-lg hover:bg-blue-600 transition-colors"
        @click="openMediaPopup"
      >
        Xem media của địa điểm
      </button>
    </div>

    <!-- Reviews Section -->
    <div class="card bg-white shadow-xl rounded-lg overflow-hidden mt-8">
      <div class="card-header bg-gradient-to-r from-blue-500 to-blue-600 text-white p-4">
        <h5 class="text-xl font-semibold">Đánh giá</h5>
      </div>

      <div class="card-body p-6">
        <div class="flex items-center mb-4">
          <h6 class="text-lg font-semibold">Đánh giá của bạn:</h6>
          <button
            class="ml-auto bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition-colors"
            @click="$emit('add-review')"
          >
            Thêm đánh giá
          </button>
        </div>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div v-for="star in 5" :key="star" class="flex items-center">
            <span class="text-yellow-400">★</span>
            <span class="ml-2">{{ star }}</span>
          </div>
        </div>
        <div class="mt-4">
          <textarea
            class="w-full p-2 border rounded-lg"
            rows="3"
            placeholder="Viết đánh giá của bạn..."
          ></textarea>
        </div>
        <button
          class="mt-2 bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition-colors"
          @click="$emit('submit-review')"
        >
          Gửi đánh giá
        </button>
      </div>

      <div class="card-body p-6">
        <div
          v-for="review in reviews"
          :key="review.id"
          class="border-b border-gray-200 py-4 last:border-b-0"
        >
          <div class="flex items-center mb-2">
            <span class="font-semibold text-lg">{{ review.userName }}</span>
            <div class="ml-3 flex">
              <span
                v-for="star in 5"
                :key="star"
                class="text-yellow-400"
                :class="{ 'opacity-30': star > review.rating }"
              >★</span>
            </div>
          </div>
          <p class="text-gray-600">{{ review.comment }}</p>
          <p class="text-sm text-gray-500 mt-1">{{ review.date }}</p>
        </div>
        <p v-if="!reviews.length" class="text-gray-500">Chưa có đánh giá nào.</p>
      </div>
    </div>

    <!-- Full Screen Modal -->
    <div
      v-if="showFullScreen"
      class="fixed inset-0 bg-black bg-opacity-80 flex items-center justify-center z-50"
    >
      <button
        class="absolute top-4 right-4 text-white text-2xl"
        @click="closeFullScreen"
      >
        ×
      </button>
      <img
        v-if="fullScreenType === 'image'"
        :src="fullScreenUrl"
        class="max-w-full max-h-full object-contain"
      />
      <video
        v-if="fullScreenType === 'video'"
        :src="fullScreenUrl"
        controls
        autoplay
        class="max-w-full max-h-full"
      ></video>
    </div>

    <!-- Media Popup Modal -->
    <div
      v-if="showMediaPopup"
      class="fixed inset-0 bg-black bg-opacity-80 flex items-center justify-center z-50"
    >
      <button
        class="absolute top-4 right-4 text-white text-2xl"
        @click="closeMediaPopup"
      >
        ×
      </button>

      <!-- Media Display -->
      <div class="relative w-full h-full flex items-center justify-center">
        <!-- Previous Button -->
        <button
          class="absolute left-4 text-white text-3xl bg-gray-800 bg-opacity-50 rounded-full w-12 h-12 flex items-center justify-center hover:bg-opacity-75"
          @click="prevMedia"
        >
          ←
        </button>

        <!-- Media Content -->
        <div class="max-w-5xl max-h-[80vh] flex items-center justify-center">
          <img
            v-if="currentMedia && currentMedia.mediaType === 1"
            :src="instance.defaults.baseURL + currentMedia.mediaUrl"
            class="max-w-full max-h-full object-contain"
          />
          <div
            v-if="currentMedia && currentMedia.mediaType === 2"
            :id="'pannellum-viewer-' + currentMedia.id"
            class="w-full h-[80vh]"
          ></div>
          <video
            v-if="currentMedia && currentMedia.mediaType === 3"
            :src="instance.defaults.baseURL + currentMedia.mediaUrl"
            controls
            autoplay
            class="max-w-full max-h-full"
          ></video>
        </div>

        <!-- Next Button -->
        <button
          class="absolute right-4 text-white text-3xl bg-gray-800 bg-opacity-50 rounded-full w-12 h-12 flex items-center justify-center hover:bg-opacity-75"
          @click="nextMedia"
        >
          →
        </button>
      </div>

      <!-- Control Bar -->
      <div class="absolute bottom-4 left-0 right-0 flex justify-center">
        <div class="bg-gray-800 bg-opacity-75 rounded-lg p-4 flex items-center gap-4">
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
        </div>
      </div>
    </div>
    </div>

    <FooterComponent />
</template>

<script setup>
import { ref, onMounted, watch, nextTick } from 'vue';
import { useRoute } from 'vue-router';
import { getPlaceByIdApi } from '@/services/modules/place.api';
import { getListMediaPlaceApi } from '@/services/modules/mediaplace.api';
import NavbarComponentV1 from '@/components/NavbarComponentV1.vue';
import FooterComponent from '@/components/FooterComponent.vue';
import instance from '@/services/axiosConfig';

// Get place ID from route
const route = useRoute();
const placeId = route.params.id;

// Reactive state for place and media
const place = ref({});
const allMedia = ref([]);
const images = ref([]);
const images360 = ref([]);
const videos = ref([]);
const audios = ref([]);

// Reviews (static for now, could be fetched from an API)
const reviews = ref([
  {
    id: 1,
    userName: 'Nguyễn Văn A',
    rating: 4,
    comment: 'Địa điểm rất đẹp và thú vị!',
    date: '2023-10-01',
  },
  {
    id: 2,
    userName: 'Trần Thị B',
    rating: 5,
    comment: 'Tôi đã có một trải nghiệm tuyệt vời tại đây!',
    date: '2023-10-02',
  },
  {
    id: 3,
    userName: 'Lê Văn C',
    rating: 3,
    comment: 'Địa điểm bình thường, không có gì đặc biệt.',
    date: '2023-10-03',
  },
  {
    id: 4,
    userName: 'Phạm Thị D',
    rating: 2,
    comment: 'Không hài lòng với dịch vụ tại đây.',
    date: '2023-10-04',
  },
  {
    id: 5,
    userName: 'Nguyễn Văn E',
    rating: 1,
    comment: 'Rất thất vọng với trải nghiệm tại đây.',
    date: '2023-10-05',
  },
]);

// Full Screen Modal State
const showFullScreen = ref(false);
const fullScreenUrl = ref('');
const fullScreenType = ref('');

// Media Popup State
const showMediaPopup = ref(false);
const currentMediaIndex = ref(0);
const currentMedia = ref(null);
const audioElement = ref(null);
const isAudioPlaying = ref(false);
const isMuted = ref(false);
const audioVolume = ref(0.5);

// Fetch place details
const fetchPlaceDetails = async () => {
  try {
    const response = await getPlaceByIdApi(placeId);
    place.value = response.data.data || {};
  } catch (error) {
    console.error('Lỗi khi tải thông tin địa điểm:', error);
  }
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
  } catch (error) {
    console.error('Lỗi khi tải danh sách media:', error);
  }
};

// Full Screen Modal Functions
const openFullScreen = (url, type) => {
  fullScreenUrl.value = url;
  fullScreenType.value = type;
  showFullScreen.value = true;
};

const closeFullScreen = () => {
  showFullScreen.value = false;
  fullScreenUrl.value = '';
  fullScreenType.value = '';
};

// Media Popup Functions
const openMediaPopup = () => {
  if (allMedia.value.length > 0) {
    showMediaPopup.value = true;
    currentMediaIndex.value = 0;
    currentMedia.value = allMedia.value[0];
    initializeAudio();
    initialize360Viewer();
  }
};

const closeMediaPopup = () => {
  showMediaPopup.value = false;
  currentMedia.value = null;
  currentMediaIndex.value = 0;
  stopAudio();
};

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

const stopAudio = () => {
  if (audioElement.value) {
    audioElement.value.pause();
    audioElement.value.currentTime = 0;
    isAudioPlaying.value = false;
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
          window.pannellum.viewer(viewerId, {
            type: 'equirectangular',
            panorama: panoramaUrl,
            autoLoad: true,
            showZoomCtrl: true,
            showFullscreenCtrl: true,
            autoRotate: -2,
          });
        } catch (error) {
          console.error('Error initializing Pannellum viewer:', error);
        }
      }, 100);
    } catch (error) {
      // console.error(`Failed to fetch image at ${panoramaUrl}:`, error);
      console.error(`Failed to fetch image at:`, error);
    }
  }
};

// Watch for changes in currentMedia to handle 360 viewer initialization
watch(currentMedia, () => {
  initialize360Viewer();
});

// Load Pannellum assets
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
  fetchMediaList();
});
</script>

<style scoped>
.card {
  transition: transform 0.2s;
}

.card:hover {
  transform: translateY(-2px);
}

input[type="range"] {
  -webkit-appearance: none;
  appearance: none;
  height: 6px;
  background: #d3d3d3;
  border-radius: 3px;
  outline: none;
}

input[type="range"]::-webkit-slider-thumb {
  -webkit-appearance: none;
  appearance: none;
  width: 16px;
  height: 16px;
  background: #ffffff;
  border: 2px solid #3b82f6;
  border-radius: 50%;
  cursor: pointer;
}

input[type="range"]::-moz-range-thumb {
  width: 16px;
  height: 16px;
  background: #ffffff;
  border: 2px solid #3b82f6;
  border-radius: 50%;
  cursor: pointer;
}
</style>