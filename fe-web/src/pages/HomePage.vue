<template>
  <NavbarComponentV1 />
  <div class="bg-gradient-to-b from-green-50 to-white">
    <!-- Header -->
    <!-- <header class="bg-white shadow-md sticky top-0 z-50">
      <div
        class="container mx-auto px-4 py-4 flex justify-between items-center"
      >
        <h1
          class="text-2xl md:text-3xl font-bold text-green-700 flex items-center"
        >
          <span class="mr-2">🌿</span> Thái Nguyên Travel
        </h1>
      </div>
    </header> -->

    <!-- Banner -->
    <Banner class="mb-8" />

    <!-- Main Content -->
    <main class="container mx-auto px-4">
      <!-- Featured Section -->
      <section class="mb-12">
        <h2
          class="text-3xl font-bold text-gray-800 mb-6 text-center animate-fade-in"
        >
          Khám Phá Thái Nguyên
        </h2>
        <div class="news-component">
          <ListCategoryBlog />
        </div>
      </section>

      <!-- Why Choose Thái Nguyên -->
      <section class="mb-12 bg-white rounded-lg shadow-lg p-8 animate-slide-up">
        <h2 class="text-3xl font-bold text-gray-800 mb-6 text-center">
          Tại Sao Chọn Thái Nguyên?
        </h2>
        <p class="text-gray-600 mb-8 text-center max-w-2xl mx-auto">
          Thái Nguyên không chỉ là một điểm đến du lịch mà còn là một hành trình
          khám phá văn hóa và thiên nhiên độc đáo.
        </p>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          <div
            v-for="(item, index) in whyChoose"
            :key="index"
            class="flex flex-col items-center text-center p-4 hover:bg-green-50 rounded-lg transition-colors duration-300"
          >
            <img
              :src="item.image"
              :alt="item.title"
              class="w-16 h-16 mb-4 object-cover rounded-full"
            />
            <h3 class="text-lg font-semibold text-gray-800 mb-2">
              {{ item.title }}
            </h3>
            <p class="text-gray-600">{{ item.description }}</p>
          </div>
        </div>
      </section>

      <!-- Travel Tips -->
      <section class="mb-12 flex flex-col lg:flex-row items-center gap-8">
        <div class="lg:w-1/2">
          <img
            src="../assets/images/image1.jpg"
            alt="Travel Tips"
            class="w-full h-80 object-cover rounded-lg shadow-md"
          />
        </div>
        <div
          class="lg:w-1/2 bg-white rounded-lg shadow-lg p-8 animate-slide-up"
        >
          <h2 class="text-3xl font-bold text-gray-800 mb-6">
            Lời Khuyên Du Lịch
          </h2>
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <div
              v-for="(tip, index) in travelTips"
              :key="index"
              class="flex items-start"
            >
              <img :src="tip.icon" :alt="tip.title" class="w-10 h-10 mr-4" />
              <div>
                <h3 class="text-lg font-semibold text-gray-800">
                  {{ tip.title }}
                </h3>
                <p class="text-gray-600">{{ tip.description }}</p>
              </div>
            </div>
          </div>
        </div>
      </section>
    </main>

    <!-- Chat Button -->
    <button @click="openChat" class="chat-button" title="Mở chat">
      <svg
        class="w-8 h-8"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z"
        />
      </svg>
    </button>

    <!-- Footer -->
    <FooterComponent />

    <!-- Chat Popup -->
    <ChatPopup :isOpen="isChatOpen" @update:isOpen="isChatOpen = $event" />
  </div>
</template>

<script setup>
import { ref } from "vue";
import NavbarComponentV1 from "@/components/NavbarComponentV1.vue";
import FooterComponent from "@/components/FooterComponent.vue";
import Banner from "@/components/Banner.vue";
import ListCategoryBlog from "@/pages/blog/ListCategoryBlog.vue";
import ChatPopup from "@/pages/chat/ChatPopup.vue";

const isChatOpen = ref(false);

const openChat = () => {
  isChatOpen.value = true;
};

const whyChoose = [
  {
    title: "🌿 Nền Văn Hóa Trà",
    description:
      "Khám phá những đồi chè xanh mướt với lịch sử canh tác hàng thế kỷ.",
    image:
      "https://i.pinimg.com/736x/d0/17/e2/d017e27bbefa4f39ece0c28cadfbdd82.jpg",
  },
  {
    title: "Vẻ Đẹp Thiên Nhiên",
    description:
      "Chiêm ngưỡng hồ nước yên bình, núi non hùng vĩ và rừng xanh bạt ngàn.",
    image: "https://images.unsplash.com/photo-1506748686214-e9df14d4d9d0",
  },
  {
    title: "Sự Đa Dạng Văn Hóa",
    description:
      "Trải nghiệm phong tục độc đáo của các dân tộc thiểu số bản địa.",
    image: "https://images.unsplash.com/photo-1519985176271-adb1088fa94c",
  },
  {
    title: "Ẩm Thực Địa Phương",
    description:
      "Thưởng thức món ăn đặc trưng từ nguyên liệu tươi ngon của núi rừng.",
    image:
      "https://i.pinimg.com/736x/c9/c8/88/c9c88801021e2b3ff435a0fc2dee7877.jpg",
  },
];

const travelTips = [
  {
    title: "Thời Gian Tốt Nhất",
    description:
      "Tháng 9 đến tháng 11 có thời tiết dễ chịu và cảnh quan tuyệt đẹp.",
    icon: "https://cdn-icons-png.flaticon.com/512/869/869869.png",
  },
  {
    title: "Di Chuyển",
    description: "Thuê xe máy hoặc taxi để dễ dàng khám phá các địa điểm.",
    icon: "https://cdn-icons-png.flaticon.com/512/744/744465.png",
  },
  {
    title: "Chỗ Ở",
    description:
      "Lựa chọn từ khách sạn hiện đại đến homestay nông thôn ấm cúng.",
    icon: "https://cdn-icons-png.flaticon.com/512/565/565517.png",
  },
  {
    title: "Chuẩn Bị Hành Lý",
    description:
      "Mang quần áo nhẹ, áo mưa, giày leo núi và thuốc chống côn trùng.",
    icon: "https://cdn-icons-png.flaticon.com/512/1019/1019607.png",
  },
];
</script>

<style>
.chat-button {
  position: fixed;
  bottom: 30px;
  right: 30px;
  padding: 14px;
  background: linear-gradient(45deg, #10b981, #34d399);
  color: white;
  border-radius: 50%;
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
  transition: all 0.3s ease;
  z-index: 1000;
}

.chat-button:hover {
  background: linear-gradient(45deg, #059669, #10b981);
  transform: translateY(-2px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
}

/* Animations */
@keyframes fade-in {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slide-up {
  from {
    transform: translateY(20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.animate-fade-in {
  animation: fade-in 1s ease-out;
}

.animate-slide-up {
  animation: slide-up 0.8s ease-out;
}
</style>
