<template>
  <div class="container mx-auto p-4">
    <div class="bg-white shadow-lg rounded-lg overflow-hidden">
      <div class="bg-gray-800 text-white px-6 py-4">
        <h5 class="text-xl font-semibold">Cập nhật Media cho Địa điểm</h5>
      </div>
      <div class="p-6">
        <!-- Ảnh -->
        <div class="mb-8">
          <h6 class="text-lg font-medium text-gray-700 mb-4">Ảnh</h6>
          <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4 mb-4">
            <div v-for="media in images" :key="media.id" class="relative group">
              <img
                :src="instance.defaults.baseURL + media.mediaUrl"
                alt="Ảnh"
                class="w-full h-32 object-cover rounded-md shadow-sm"
              />
              <button
                @click="deleteMedia(media.id)"
                class="absolute top-2 right-2 bg-red-600 text-white rounded-full p-1 opacity-0 group-hover:opacity-100 transition-opacity duration-200"
              >
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
            </div>
          </div>
          <label class="block">
            <span class="sr-only">Chọn ảnh</span>
            <input
              type="file"
              accept="image/*"
              @change="handleFileChange($event, 1)"
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100 cursor-pointer"
            />
          </label>
        </div>

        <!-- Ảnh 360 -->
        <div class="mb-8">
          <h6 class="text-lg font-medium text-gray-700 mb-4">Ảnh 360</h6>
          <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4 mb-4">
            <div v-for="media in images360" :key="media.id" class="relative group">
              <img
                :src="instance.defaults.baseURL + media.mediaUrl"
                alt="Ảnh 360"
                class="w-full h-32 object-cover rounded-md shadow-sm"
              />
              <button
                @click="deleteMedia(media.id)"
                class="absolute top-2 right-2 bg-red-600 text-white rounded-full p-1 opacity-0 group-hover:opacity-100 transition-opacity duration-200"
              >
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
            </div>
          </div>
          <label class="block">
            <span class="sr-only">Chọn ảnh 360</span>
            <input
              type="file"
              accept="image/*"
              @change="handleFileChange($event, 2)"
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100 cursor-pointer"
            />
          </label>
        </div>

        <!-- Video -->
        <div class="mb-8">
          <h6 class="text-lg font-medium text-gray-700 mb-4">Video</h6>
          <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4 mb-4">
            <div v-for="media in videos" :key="media.id" class="relative group">
              <video
                :src="instance.defaults.baseURL + media.mediaUrl"
                controls
                class="w-full h-48 object-cover rounded-md shadow-sm"
              ></video>
              <button
                @click="deleteMedia(media.id)"
                class="absolute top-2 right-2 bg-red-600 text-white rounded-full p-1 opacity-0 group-hover:opacity-100 transition-opacity duration-200"
              >
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
            </div>
          </div>
          <label class="block">
            <span class="sr-only">Chọn video</span>
            <input
              type="file"
              accept="video/*"
              @change="handleFileChange($event, 3)"
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100 cursor-pointer"
            />
          </label>
        </div>

        <!-- Audio -->
        <div class="mb-8">
          <h6 class="text-lg font-medium text-gray-700 mb-4">Audio</h6>
          <div v-if="audios.length" class="relative group mb-4">
            <audio
              :src="instance.defaults.baseURL + audios[0].mediaUrl"
              controls
              class="w-full"
            ></audio>
            <button
              @click="deleteMedia(audios[0].id)"
              class="absolute top-2 right-2 bg-red-600 text-white rounded-full p-1 opacity-0 group-hover:opacity-100 transition-opacity duration-200"
            >
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
          <label class="block">
            <span class="sr-only">Chọn audio</span>
            <input
              type="file"
              accept="audio/*"
              @change="handleFileChange($event, 4)"
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100 cursor-pointer"
              :disabled="audios.length > 0"
            />
          </label>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { getListMediaPlaceApi, deleteMediaPlaceApi } from '@/apis/modules/mediaplace.api';
import { uploadSingleFileApi } from '@/apis/modules/upload.api';
import instance from '@/apis/axiosConfig';

const route = useRoute();
const placeId = route.params.id;

// Danh sách media phân loại
const images = ref([]);
const images360 = ref([]);
const videos = ref([]);
const audios = ref([]);

// Tải danh sách media và phân loại
const fetchMediaList = async () => {
  try {
    const response = await getListMediaPlaceApi(placeId);
    if (response.code === 200) {
      const mediaData = JSON.parse(response.data);
      images.value = mediaData.filter(item => item.mediaType === 1);
      images360.value = mediaData.filter(item => item.mediaType === 2);
      videos.value = mediaData.filter(item => item.mediaType === 3);
      audios.value = mediaData.filter(item => item.mediaType === 4);
    }
  } catch (error) {
    console.error('Lỗi khi tải danh sách media:', error);
  }
};

// Xử lý tải lên file
const handleFileChange = async (event, mediaType) => {
  const file = event.target.files[0];
  if (!file) return;

  const formData = new FormData();
  formData.append('file', file);
  formData.append('type', mediaType.toString());
  formData.append('imagefor', 'place-media');
  formData.append('placeId', placeId);

  try {
    const uploadResponse = await uploadSingleFileApi(formData);
    if (uploadResponse && uploadResponse.data && uploadResponse.data.id) {
      await fetchMediaList();
    } else {
      throw new Error('Không nhận được phản hồi hợp lệ từ server.');
    }
  } catch (error) {
    console.error('Lỗi khi tải lên file:', error);
  }
};

// Xóa media
const deleteMedia = async (mediaId) => {
  try {
    await deleteMediaPlaceApi(mediaId);
    await fetchMediaList();
  } catch (error) {
    console.error('Lỗi khi xóa media:', error);
  }
};

onMounted(() => {
  fetchMediaList();
});
</script>

<style>
@import 'https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css';
</style>