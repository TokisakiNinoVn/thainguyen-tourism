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

    <!-- Media Card -->
    <div class="card bg-white shadow-xl rounded-lg overflow-hidden mt-8">
      <div class="card-header bg-gradient-to-r from-blue-500 to-blue-600 text-white p-4">
        <h5 class="text-xl font-semibold">Cập nhật media cho địa điểm</h5>
      </div>
      <div class="card-body p-6">
        <!-- Images -->
        <div class="mb-6">
          <h6 class="text-lg font-semibold mb-3">Ảnh</h6>
          <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
            <div v-for="media in images" :key="media.id">
              <img
                :src="instance.defaults.baseURL + media.mediaUrl"
                alt="Ảnh"
                class="w-full h-32 object-cover rounded-lg shadow-sm cursor-pointer hover:opacity-80 transition-opacity"
                @click="openFullScreen(instance.defaults.baseURL + media.mediaUrl, 'image')"
              />
            </div>
          </div>
        </div>

        <!-- 360 Images -->
        <div class="mb-6">
          <h6 class="text-lg font-semibold mb-3">Ảnh 360</h6>
          <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
            <div v-for="media in images360" :key="media.id">
              <img
                :src="instance.defaults.baseURL + media.mediaUrl"
                alt="Ảnh 360"
                class="w-full h-32 object-cover rounded-lg shadow-sm cursor-pointer hover:opacity-80 transition-opacity"
                @click="open360Viewer(instance.defaults.baseURL + media.mediaUrl)"
              />
            </div>
          </div>
        </div>

        <!-- Videos -->
        <div class="mb-6">
          <h6 class="text-lg font-semibold mb-3">Video</h6>
          <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
            <div v-for="media in videos" :key="media.id">
              <video
                :src="instance.defaults.baseURL + media.mediaUrl"
                controls
                class="w-full h-48 rounded-lg shadow-sm cursor-pointer"
                @click="openFullScreen(instance.defaults.baseURL + media.mediaUrl, 'video')"
              ></video>
            </div>
          </div>
        </div>

        <!-- Audio -->
        <div class="mb-6">
          <h6 class="text-lg font-semibold mb-3">Audio</h6>
          <div v-if="audios.length" class="bg-gray-100 p-4 rounded-lg">
            <audio
              :src="instance.defaults.baseURL + audios[0].mediaUrl"
              controls
              class="w-full"
            ></audio>
          </div>
          <p v-else class="text-gray-500">Không có audio nào.</p>
        </div>
      </div>
    </div>

    <!-- Reviews Section -->
    <div class="card bg-white shadow-xl rounded-lg overflow-hidden mt-8">
      <div class="card-header bg-gradient-to-r from-blue-500 to-blue-600 text-white p-4">
        <h5 class="text-xl font-semibold">Đánh giá</h5>
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
                >★</span
              >
            </div>
          </div>
          <p class="text-gray-600">{{ review.comment }}</p>
          <p class="text-sm text-gray-500 mt-1">{{ review.date }}</p>
        </div>
        <p v-if="!reviews.length" class="text-gray-500">Chưa có đánh giá nào.</p>
      </div>
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

  <!-- 360 Viewer Modal -->
  <div
    v-if="show360Viewer"
    class="fixed inset-0 bg-black bg-opacity-80 flex items-center justify-center z-50"
  >
    <button
      class="absolute top-4 right-4 text-white text-2xl"
      @click="close360Viewer"
    >
      ×
    </button>
    <div id="pannellum-viewer" class="w-full h-full"></div>
  </div>

  <FooterComponent />
</template>

<script setup>
import { ref, onMounted } from 'vue';
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

// 360 Viewer State
const show360Viewer = ref(false);
const current360Image = ref('');

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

// 360 Viewer Functions
const open360Viewer = (url) => {
  current360Image.value = url;
  show360Viewer.value = true;
  setTimeout(() => {
    window.pannellum.viewer('pannellum-viewer', {
      type: 'equirectangular',
      panorama: url,
      autoLoad: true,
      showZoomCtrl: true,
      showFullscreenCtrl: true,
      autoRotate: -2,
    });
  }, 0);
};

const close360Viewer = () => {
  show360Viewer.value = false;
  current360Image.value = '';
};

// Load data on component mount
onMounted(() => {
  fetchPlaceDetails();
  fetchMediaList();
});
</script>

<style scoped>
/* Optional: Add scoped styles for better encapsulation */
.card {
  transition: transform 0.2s;
}

.card:hover {
  transform: translateY(-2px);
}
</style>