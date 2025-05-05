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
      >
        <router-link to="/mediaplace" class="text-white font-semibold">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5 inline-block ml-2"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M13 7h-1v6h1m0 4h-1v2h1m4-10h-1v6h1m0 4h-1v2h1m4-10h-1v6h1m0 4h-1v2h1M3 7h1v6H3m0 4h1v2H3m4-10h1v6H7m0 4h1v2H7m4-10h1v6h-1m0 4h1v2h-1M3 3l18 18M3 21L21 3"
            />
          </svg>
          Xem media của địa điểm
        </router-link>
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
            @click="showReviewForm = true"
          >
            Thêm đánh giá
          </button>
        </div>
        
        <!-- Review Form -->
        <div v-if="showReviewForm" class="mb-4">
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

        <!-- Reviews List -->
        <div
          v-for="review in reviews"
          :key="review.id"
          class="border-b border-gray-200 py-4 last:border-b-0"
        >
          <div class="flex items-center mb-2">
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
  </div>

  <FooterComponent />
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { getPlaceByIdApi, getReviewPlaceByIdApi } from '@/services/modules/place.api';
import { createReviewApi } from '@/services/modules/review.api';
import NavbarComponentV1 from '@/components/NavbarComponentV1.vue';
import FooterComponent from '@/components/FooterComponent.vue';
import instance from '@/services/axiosConfig';

// Get place ID from route
const route = useRoute();
const placeId = route.params.id;

// Reactive state
const place = ref({});
const reviews = ref([]);
const showFullScreen = ref(false);
const fullScreenUrl = ref('');
const fullScreenType = ref('');
const showReviewForm = ref(false);
const newReview = ref({
  rating: 0,
  comment: ''
});

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
    reviews.value = JSON.parse(response.data) || [];
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
const formatDate = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
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

onMounted(() => {
  fetchPlaceDetails();
  fetchReviews();
});
</script>

<style scoped>
.fixed.inset-0 {
  display: flex;
  align-items: center;
  justify-content: center;
}
button {
  transition: background-color 0.3s ease;
}
.prose {
  line-height: 1.75;
}
textarea {
  resize: vertical;
  min-height: 80px;
}
</style>