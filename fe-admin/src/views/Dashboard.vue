<template>
  <div class="main-blog__content">
    <div
      class="w-[90vw] h-[90vh] rounded-xl shadow-2xl flex flex-col relative overflow-hidden p-6 bg-gradient-to-br from-blue-50 to-indigo-50"
    >
      <!-- Header -->
      <h1 class="text-3xl font-bold text-gray-800 mb-6">Bảng Điều Khiển</h1>

      <!-- Stats Cards -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
        <div
          class="bg-white p-5 rounded-xl shadow-lg flex items-center transform hover:scale-105 transition-transform duration-300"
        >
          <div class="p-3 rounded-full bg-blue-100 mr-4">
            <i class="fas fa-users text-blue-600 text-2xl"></i>
          </div>
          <div>
            <p class="text-sm text-gray-500">Tổng Người Dùng</p>
            <p class="text-2xl font-semibold text-gray-800">{{ userCount }}</p>
          </div>
        </div>
        <div
          class="bg-white p-5 rounded-xl shadow-lg flex items-center transform hover:scale-105 transition-transform duration-300"
        >
          <div class="p-3 rounded-full bg-green-100 mr-4">
            <i class="fas fa-star text-green-600 text-2xl"></i>
          </div>
          <div>
            <p class="text-sm text-gray-500">Tổng Đánh Giá</p>
            <p class="text-2xl font-semibold text-gray-800">
              {{ reviewCount }}
            </p>
          </div>
        </div>
        <div
          class="bg-white p-5 rounded-xl shadow-lg flex items-center transform hover:scale-105 transition-transform duration-300"
        >
          <div class="p-3 rounded-full bg-purple-100 mr-4">
            <i class="fas fa-book text-purple-600 text-2xl"></i>
          </div>
          <div>
            <p class="text-sm text-gray-500">Tổng Bài Viết</p>
            <p class="text-2xl font-semibold text-gray-800">{{ blogCount }}</p>
          </div>
        </div>
      </div>

      <!-- Chart Section -->
      <div class="bg-white p-6 rounded-xl shadow-lg mb-6">
        <h2 class="text-lg font-semibold text-gray-800 mb-4">
          Lượt Truy Cập Hàng Tháng
        </h2>
        <apexchart
          type="area"
          height="300"
          :options="chartOptions"
          :series="series"
        ></apexchart>
      </div>

      <!-- Favorite Places -->
      <div class="bg-white p-6 rounded-xl shadow-lg">
        <h2 class="text-lg font-semibold text-gray-800 mb-4">
          Địa Điểm Yêu Thích
        </h2>
        <ul class="space-y-3">
          <li
            v-for="place in listFavoritePlaces"
            :key="place.name"
            class="flex justify-between items-center p-2 rounded-lg hover:bg-gray-100 transition-colors"
          >
            <span class="text-gray-600 flex items-center">
              <i class="fas fa-map-marker-alt text-red-500 mr-2"></i>
              {{ place.name }}
            </span>
            <span
              class="text-gray-800 font-semibold bg-blue-100 px-2 py-1 rounded"
              >{{ place.count }}</span
            >
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import {
  getVisitCountApi,
  getBlogCountApi,
  getUserCountApi,
  getReviewCountApi,
  getListFavoritePlacesApi,
} from "@/apis/modules/dasboard.api.js";

const visitCount = ref([]);
const blogCount = ref(0);
const userCount = ref(0);
const reviewCount = ref(0);
const listFavoritePlaces = ref([]);

const chartOptions = ref({
  chart: {
    type: "area",
    height: 300,
    toolbar: {
      show: false,
    },
    animations: {
      enabled: true,
      easing: "easeinout",
      speed: 800,
    },
  },
  dataLabels: {
    enabled: false,
  },
  stroke: {
    curve: "smooth",
    width: 3,
  },
  xaxis: {
    categories: [],
    title: {
      text: "Tháng",
      style: {
        fontSize: "14px",
        fontWeight: 600,
      },
    },
  },
  yaxis: {
    title: {
      text: "Số Lượt Truy Cập",
      style: {
        fontSize: "14px",
        fontWeight: 600,
      },
    },
  },
  fill: {
    type: "gradient",
    gradient: {
      shadeIntensity: 1,
      opacityFrom: 0.7,
      opacityTo: 0.3,
      stops: [0, 90, 100],
    },
  },
  colors: ["#3B82F6"],
  tooltip: {
    y: {
      formatter: (val) => `${val} lượt truy cập`,
    },
  },
});

const series = ref([
  {
    name: "Lượt Truy Cập",
    data: [],
  },
]);

const fecthBlogCount = async () => {
  try {
    const response = await getBlogCountApi();
    blogCount.value = response.data.blogCount;
  } catch (error) {
    console.error("Lỗi khi lấy số lượng bài viết:", error);
  }
};

const fecthUserCount = async () => {
  try {
    const response = await getUserCountApi();
    userCount.value = response.data.userCount;
  } catch (error) {
    console.error("Lỗi khi lấy số lượng người dùng:", error);
  }
};

const fecthReviewCount = async () => {
  try {
    const response = await getReviewCountApi();
    reviewCount.value = response.data.reviewCount;
  } catch (error) {
    console.error("Lỗi khi lấy số lượng đánh giá:", error);
  }
};

const fecthListFavoritePlaces = async () => {
  try {
    const response = await getListFavoritePlacesApi();
    listFavoritePlaces.value = JSON.parse(response.data);
  } catch (error) {
    console.error("Lỗi khi lấy danh sách địa điểm yêu thích:", error);
  }
};

const fecthVisitCount = async () => {
  try {
    const response = await getVisitCountApi();
    visitCount.value = response.data;
  } catch (error) {
    console.error("Lỗi khi lấy số lượt truy cập:", error);
  }
};

const initChart = () => {
  chartOptions.value.xaxis.categories = visitCount.value.map((item) =>
    new Date(
      `20${item.month.slice(2, 4)}-${item.month.slice(0, 2)}-01`
    ).toLocaleString("default", {
      month: "short",
      year: "numeric",
    })
  );
  series.value[0].data = visitCount.value.map((item) => item.count);
};

onMounted(async () => {
  await Promise.all([
    fecthVisitCount(),
    fecthBlogCount(),
    fecthUserCount(),
    fecthReviewCount(),
    fecthListFavoritePlaces(),
  ]);
  if (visitCount.value.length) {
    initChart();
  }
});
</script>
