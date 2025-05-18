<template>
  <div class="container mx-auto px-4 py-8">
    <!-- Tiêu đề và ô tìm kiếm -->
    <div class="flex flex-col md:flex-row justify-between items-center mb-8">
      <h2 class="text-3xl font-bold text-gray-800">🗺️ Các địa điểm</h2>
    </div>

    <!-- Danh sách địa điểm -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <div
        v-for="(place, index) in displayedPlaces"
        :key="place.id"
        class="bg-white rounded-xl shadow-lg overflow-hidden transform transition-all duration-300 hover:scale-105 hover:shadow-xl"
        :class="{ hidden: index >= 6 && !showAll }"
      >
        <router-link :to="`/places/${place.id}`" class="block">
          <img
            :src="
              place.imageUrl
                ? instance.defaults.baseURL + place.imageUrl
                : 'https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg'
            "
            :alt="place.name"
            class="h-56 w-full object-cover"
          />
          <div class="p-5">
            <h3 class="text-xl font-semibold text-gray-800 mb-2 line-clamp-1">
              {{ place.name }}
            </h3>
            <p
              class="text-gray-600 mb-3 line-clamp-3"
              v-html="getShortDescription(place.description)"
            ></p>

            <!-- Rating và Reviews -->
            <div class="flex items-center mb-3">
              <div class="flex text-yellow-400">
                <i
                  v-for="n in Math.floor(place.averageRating)"
                  :key="'star-full-' + n"
                  class="fa-solid fa-star"
                ></i>
                <i
                  v-if="place.averageRating % 1 >= 0.5"
                  class="fa-solid fa-star-half-stroke"
                ></i>
                <i
                  v-for="n in 5 - Math.ceil(place.averageRating)"
                  :key="'star-empty-' + n"
                  class="fa-regular fa-star"
                ></i>
              </div>
              <span class="ml-2 text-gray-600 text-sm"
                >({{ place.totalReviews }} đánh giá)</span
              >
            </div>

            <!-- Link Google Maps -->
            <a
              :href="`https://www.google.com/maps?q=${place.latitude},${place.longitude}`"
              target="_blank"
              class="text-blue-600 hover:text-blue-800 text-sm flex items-center mb-4"
            >
              <i class="fa-solid fa-map-location-dot mr-2"></i>
              Xem trên Google Maps
            </a>

            <!-- Nút xem chi tiết và lưu yêu thích -->
            <div class="flex justify-between items-center">
              <button
                class="bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700 transition-colors duration-300"
              >
                Xem chi tiết
              </button>
              <button
                @click.stop.prevent="toggleFavorite(place)"
                class="text-2xl"
                :title="place.isFavorite ? 'Bỏ yêu thích' : 'Thêm yêu thích'"
              >
                <i
                  :class="[
                    place.isFavorite
                      ? 'fa-solid fa-heart text-red-500'
                      : 'fa-regular fa-heart text-gray-500',
                    'hover:text-red-700 transition-colors duration-300',
                  ]"
                ></i>
              </button>
            </div>
          </div>
        </router-link>
      </div>
    </div>

    <!-- Nút Xem thêm -->
    <div v-if="filteredPlaces.length > 6" class="text-center mt-8">
      <button
        @click="showAll = !showAll"
        class="bg-blue-600 text-white py-2 px-6 rounded-lg hover:bg-blue-700 transition-colors duration-300"
      >
        {{ showAll ? "Thu gọn" : "Xem thêm" }}
      </button>
    </div>

    <!-- Thông báo không tìm thấy -->
    <div v-if="filteredPlaces.length === 0" class="text-center py-10">
      <p class="text-gray-600 text-lg">Không tìm thấy địa điểm nào phù hợp.</p>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, computed } from "vue";
import { getListPlaceApi } from "@/services/modules/place.api";
import { postSavePlaceApi } from "@/services/modules/save.api";
import instance from "@/services/axiosConfig";

const listPlace = ref([]);
const showAll = ref(false);

// Computed property để lọc danh sách hiển thị
const filteredPlaces = computed(() => {
  return listPlace.value;
});

// Computed property để giới hạn số lượng place hiển thị
const displayedPlaces = computed(() => {
  return showAll.value
    ? filteredPlaces.value
    : filteredPlaces.value.slice(0, 6);
});

// Lấy danh sách địa điểm
const getListPlace = async () => {
  try {
    const response = await getListPlaceApi();
    listPlace.value = response.data.data.map((place) => ({
      ...place,
      isFavorite: false,
    }));
  } catch (error) {
    console.error("Error fetching list of places:", error);
  }
};

// Cắt mô tả ngắn
const getShortDescription = (htmlDescription) => {
  const tempEl = document.createElement("div");
  tempEl.innerHTML = htmlDescription;
  const text = tempEl.textContent || tempEl.innerText || "";
  return text.length > 150 ? text.slice(0, 150) + "..." : text;
};

// Toggle trạng thái yêu thích
const toggleFavorite = async (place) => {
  try {
    await postSavePlaceApi({ placeId: place.id });
    place.isFavorite = !place.isFavorite;
    alert(
      place.isFavorite
        ? "Đã thêm vào danh sách yêu thích!"
        : "Đã bỏ khỏi danh sách yêu thích!"
    );
  } catch (error) {
    console.error("Error toggling favorite:", error);
    alert("Có lỗi xảy ra khi lưu địa điểm. Vui lòng thử lại!");
  }
};

onMounted(() => {
  getListPlace();
});
</script>

<style scoped>
.line-clamp-1 {
  display: -webkit-box;
  -webkit-line-clamp: 1;
  line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.line-clamp-3 {
  display: -webkit-box;
  -webkit-line-clamp: 3;
  line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  min-height: 72px;
}
</style>
