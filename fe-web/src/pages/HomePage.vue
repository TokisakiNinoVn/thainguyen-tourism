<template>
  <NavbarComponentV1 />

  <div class="bg-gradient-to-b from-green-50 to-white">
    <!-- Hero Banner -->
    <section
      class="relative bg-cover bg-center h-[80vh] flex items-center justify-center text-center"
      style="
        background-image: url('https://toquoc.mediacdn.vn/Uploaded/minhkhanh/2018_08_19/chef/BINM4708_AOAV.jpg');
      "
    >
      <div class="absolute inset-0 bg-black/60"></div>
      <div class="relative z-10 text-white px-4">
        <h1
          class="text-5xl md:text-6xl font-bold tracking-tight animate-fadeIn"
        >
          Chào Mừng Đến Với Thái Nguyên
        </h1>
        <p
          class="mt-4 text-xl md:text-2xl max-w-3xl mx-auto animate-fadeIn delay-200"
        >
          Khám phá những đồi chè xanh mướt, văn hóa dân tộc phong phú và thiên
          nhiên hùng vĩ tại “thủ phủ chè” của Việt Nam.
        </p>
        <router-link
          to="/register"
          class="mt-6 inline-block bg-green-600 hover:bg-green-700 text-white font-semibold py-3 px-8 rounded-lg transition duration-300 transform hover:-translate-y-1 animate-fadeIn delay-400"
        >
          Bắt Đầu Hành Trình
        </router-link>
      </div>
    </section>

    <!-- Main Content -->
    <main class="container mx-auto px-4 md:px-12 py-12">
      <!-- Featured Attractions -->
      <section class="mb-16">
        <h2
          class="text-3xl md:text-4xl font-bold text-green-800 mb-8 text-center animate-slide-up"
        >
          Điểm Đến Nổi Bật
        </h2>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          <div
            v-for="(attraction, index) in featuredAttractions"
            :key="index"
            class="bg-white/95 backdrop-blur-sm rounded-xl shadow-lg overflow-hidden hover:shadow-xl transition duration-300 transform hover:-translate-y-2"
          >
            <img
              :src="attraction.image"
              :alt="attraction.title"
              class="w-full h-48 object-cover"
            />
            <div class="p-6">
              <h3 class="text-xl font-semibold text-gray-800 mb-2">
                {{ attraction.title }}
              </h3>
              <p class="text-gray-600">{{ attraction.description }}</p>
              <router-link
                to="/attractions"
                class="mt-4 inline-block text-green-600 hover:text-green-700 font-semibold"
              >
                Khám Phá Thêm
              </router-link>
            </div>
          </div>
        </div>

        <ListPlaceComponnet />
      </section>

      <!-- Blog/Travel Stories -->
      <section class="mb-16 bg-green-50 rounded-xl py-12 px-8">
        <h2
          class="text-3xl md:text-4xl font-bold text-green-800 mb-8 text-center animate-slide-up"
        >
          Câu Chuyện Du Lịch
        </h2>
        <div class="news-component">
          <ListCategoryBlog />
        </div>
        <div class="text-center mt-8">
          <router-link
            to="/blogs"
            class="inline-block bg-green-600 hover:bg-green-700 text-white font-semibold py-3 px-8 rounded-lg transition duration-300"
          >
            Xem Tất Cả Bài Viết
          </router-link>
        </div>
      </section>

      <!-- Why Choose Thái Nguyên -->
      <section class="mb-16">
        <h2
          class="text-3xl md:text-4xl font-bold text-green-800 mb-8 text-center animate-slide-up"
        >
          Tại Sao Chọn Thái Nguyên?
        </h2>
        <p
          class="text-gray-600 mb-8 text-center max-w-3xl mx-auto animate-slide-up delay-200"
        >
          Thái Nguyên không chỉ là một điểm đến du lịch, mà còn là hành trình
          khám phá thiên nhiên, văn hóa, và những trải nghiệm khó quên.
        </p>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          <div
            v-for="(item, index) in whyChoose"
            :key="index"
            class="bg-white/95 backdrop-blur-sm p-6 rounded-xl shadow-lg hover:shadow-xl transition duration-300 transform hover:-translate-y-2"
          >
            <img
              :src="item.image"
              :alt="item.title"
              class="w-16 h-16 mb-4 object-cover rounded-full mx-auto"
            />
            <h3 class="text-lg font-semibold text-gray-800 mb-2 text-center">
              {{ item.title }}
            </h3>
            <p class="text-gray-600 text-center">{{ item.description }}</p>
          </div>
        </div>
      </section>

      <!-- Travel Tips -->
      <section class="mb-16 flex flex-col lg:flex-row items-center gap-8">
        <div class="lg:w-1/2">
          <img
            src="https://123tadi.com/wp-content/Content/images/khek0.jpg"
            alt="Thác nước Khe Cả"
            class="w-full h-96 object-cover rounded-xl shadow-lg transform hover:scale-105 transition duration-500"
          />
        </div>
        <div
          class="lg:w-1/2 bg-white/95 backdrop-blur-sm rounded-xl shadow-lg p-8 animate-slide-up"
        >
          <h2 class="text-3xl font-bold text-green-800 mb-6">
            Mẹo Du Lịch Thái Nguyên
          </h2>
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <div
              v-for="(tip, index) in travelTips"
              :key="index"
              class="flex items-start"
            >
              <img
                :src="tip.icon"
                :alt="tip.title"
                class="w-10 h-10 mr-4 flex-shrink-0"
              />
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

      <!-- Testimonials -->
      <section class="mb-16 bg-green-50 rounded-xl py-12 px-8">
        <h2
          class="text-3xl md:text-4xl font-bold text-green-800 mb-8 text-center animate-slide-up"
        >
          Cảm Nhận Du Khách
        </h2>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div
            v-for="(testimonial, index) in testimonials"
            :key="index"
            class="bg-white/95 backdrop-blur-sm p-6 rounded-xl shadow-lg hover:shadow-xl transition duration-300"
          >
            <p class="text-gray-600 italic mb-4">“{{ testimonial.quote }}”</p>
            <div class="flex items-center">
              <img
                :src="testimonial.avatar"
                :alt="testimonial.name"
                class="w-12 h-12 rounded-full mr-4"
              />
              <div>
                <p class="text-gray-800 font-semibold">
                  {{ testimonial.name }}
                </p>
                <p class="text-gray-600 text-sm">{{ testimonial.location }}</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Call to Action -->
      <section class="text-center py-12 bg-green-600 rounded-xl text-white">
        <h2 class="text-3xl font-bold mb-4">Sẵn Sàng Khám Phá Thái Nguyên?</h2>
        <p class="mb-6 max-w-xl mx-auto">
          Đăng ký ngay để nhận thông tin mới nhất về điểm đến, lễ hội, và ưu đãi
          du lịch!
        </p>
        <router-link
          to="/register"
          class="inline-block bg-white text-green-600 hover:bg-gray-100 font-semibold py-3 px-8 rounded-lg transition duration-300"
        >
          Đăng Ký Ngay
        </router-link>
      </section>
    </main>

    <!-- Chat Button -->
    <button
      @click="openChat"
      class="fixed bottom-8 right-8 p-4 bg-gradient-to-r from-green-500 to-green-600 text-white rounded-full shadow-lg hover:shadow-xl transition duration-300 transform hover:-translate-y-1 z-50"
      title="Mở chat"
    >
      <svg
        class="w-6 h-6"
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

    <FooterComponent />

    <!-- Chat Popup -->
    <ChatPopup :isOpen="isChatOpen" @update:isOpen="isChatOpen = $event" />
  </div>
</template>

<script setup>
import { ref } from "vue";
import NavbarComponentV1 from "@/components/NavbarComponentV1.vue";
import FooterComponent from "@/components/FooterComponent.vue";
// import Banner from "@/components/Banner.vue";
import ListCategoryBlog from "@/pages/blog/ListCategoryBlog.vue";
import ListPlaceComponnet from "@/pages/place/ListPlaceComponnet.vue";
import ChatPopup from "@/pages/chat/ChatPopup.vue";

const isChatOpen = ref(false);

const openChat = () => {
  isChatOpen.value = true;
};

const featuredAttractions = [
  {
    title: "Đồi Chè Tân Cương",
    description:
      "Thưởng thức không khí trong lành giữa những cánh đồng chè xanh mướt.",
    image:
      "https://toquoc.mediacdn.vn/Uploaded/minhkhanh/2018_08_19/chef/BINM4708_AOAV.jpg",
  },
  {
    title: "Hồ Núi Cốc",
    description:
      "Hồ nước thơ mộng với truyền thuyết tình yêu và cảnh quan tuyệt đẹp.",
    image:
      "https://khamphadisan.com.vn/wp-content/uploads/2016/05/ho-nui-coc-4.jpg",
  },
  {
    title: "Thác Khe Cả",
    description: "Khám phá thác nước hùng vĩ giữa thiên nhiên hoang sơ.",
    image: "https://123tadi.com/wp-content/Content/images/khek0.jpg",
  },
  {
    title: "Chùa Hang",
    description:
      "Ngôi chùa cổ kính với kiến trúc độc đáo và không gian tâm linh.",
    image:
      "https://luhanhvietnam.com.vn/du-lich/vnt_upload/news/03_2023/chua-hang-thai-nguyen-9.jpg",
  },
];

const whyChoose = [
  {
    title: "Nền Văn Hóa Trà",
    description:
      "Khám phá lịch sử canh tác chè hàng thế kỷ tại Tân \n" + "Tân Cương.",
    image:
      "https://i.pinimg.com/736x/d0/17/e2/d017e27bbefa4f39ece0c28cadfbdd82.jpg",
  },
  {
    title: "Thiên Nhiên Hùng Vĩ",
    description: "Chiêm ngưỡng hồ nước, núi non và rừng xanh tuyệt đẹp.",
    image: "https://images.unsplash.com/photo-1506748686214-e9df14d4d9d0",
  },
  {
    title: "Văn Hóa Đa Dạng",
    description:
      "Trải nghiệm phong tục độc đáo của các dân tộc Tày, Nùng, Thái.",
    image: "https://images.unsplash.com/photo-1519985176271-adb1088fa94c",
  },
  {
    title: "Ẩm Thực Đặc Sắc",
    description: "Thưởng thức cơm lam, bánh chưng Bờ Đậu và xôi trám độc đáo.",
    image:
      "https://i.pinimg.com/736x/c9/c8/88/c9c88801021e2b3ff435a0fc2dee7877.jpg",
  },
];

const travelTips = [
  {
    title: "Thời Gian Lý Tưởng",
    description: "Tháng 9-11 có thời tiết mát mẻ và cảnh quan tuyệt đẹp.",
    icon: "https://cdn-icons-png.flaticon.com/512/869/869869.png",
  },
  {
    title: "Phương Tiện Di Chuyển",
    description: "Thuê xe máy hoặc taxi để khám phá các điểm đến dễ dàng.",
    icon: "https://cdn-icons-png.flaticon.com/512/744/744465.png",
  },
  {
    title: "Lựa Chọn Chỗ Ở",
    description: "Từ khách sạn hiện đại đến homestay nông thôn ấm cúng.",
    icon: "https://cdn-icons-png.flaticon.com/512/565/565517.png",
  },
  {
    title: "Chuẩn Bị Hành Lý",
    description: "Mang giày leo núi, áo mưa và thuốc chống côn trùng.",
    icon: "https://cdn-icons-png.flaticon.com/512/1019/1019607.png",
  },
];

const testimonials = [
  {
    name: "Nguyễn Minh Anh",
    location: "Hà Nội",
    quote:
      "Đồi chè Tân Cương đẹp như tranh vẽ, không khí trong lành và người dân rất thân thiện!",
    avatar: "https://randomuser.me/api/portraits/women/44.jpg",
  },
  {
    name: "Trần Văn Hùng",
    location: "TP.HCM",
    quote: "Hồ Núi Cốc là nơi tuyệt vời để thư giãn và tận hưởng thiên nhiên.",
    avatar: "https://randomuser.me/api/portraits/men/32.jpg",
  },
  {
    name: "Lê Thị Mai",
    location: "Đà Nẵng",
    quote: "Ẩm thực Thái Nguyên thật độc đáo, nhất là cơm lam và xôi trám!",
    avatar: "https://randomuser.me/api/portraits/women/68.jpg",
  },
];
</script>

<style scoped>
/* Animations */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
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

.animate-fadeIn {
  animation: fadeIn 0.8s ease-out;
}

.animate-slide-up {
  animation: slide-up 0.8s ease-out;
}

.delay-200 {
  animation-delay: 0.2s;
}

.delay-400 {
  animation-delay: 0.4s;
}

/* Responsive Design */
@media (max-width: 768px) {
  .text-5xl {
    font-size: 2.5rem;
  }
  .text-6xl {
    font-size: 3rem;
  }
  .h-96 {
    height: 16rem;
  }
}
</style>
