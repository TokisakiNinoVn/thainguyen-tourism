<template>
  <NavbarComponentV1 />

  <!-- Hero Section -->
  <section
    class="relative bg-cover bg-center h-[70vh] flex items-center justify-center text-center"
    style="
      background-image: url('https://images.unsplash.com/photo-1507525428034-b723cf961d3e?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80');
    "
  >
    <div class="absolute inset-0 bg-black/60"></div>
    <div class="relative z-10 text-white px-4">
      <h1 class="text-5xl md:text-6xl font-bold tracking-tight animate-fadeIn">
        Du Lịch Thái Nguyên Thông Minh
      </h1>
      <p
        class="mt-4 text-xl md:text-2xl max-w-2xl mx-auto animate-fadeIn delay-200"
      >
        Khám phá vẻ đẹp thiên nhiên hùng vĩ, văn hóa đa dạng và những trải
        nghiệm độc đáo tại Thái Nguyên.
      </p>
      <router-link
        to="/register"
        class="mt-6 inline-block bg-green-600 hover:bg-green-700 text-white font-semibold py-3 px-8 rounded-lg transition duration-300 transform hover:-translate-y-1 animate-fadeIn delay-400"
      >
        Bắt Đầu Hành Trình
      </router-link>
    </div>
  </section>

  <div class="container mx-auto p-6 md:p-12">
    <!-- About Thái Nguyên -->
    <section class="mb-16">
      <h2
        class="text-3xl md:text-4xl font-bold text-green-800 mb-6 text-center"
      >
        Giới Thiệu Về Thái Nguyên
      </h2>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
        <div class="text-gray-700 leading-relaxed">
          <p class="mb-4">
            Thái Nguyên, nằm ở vùng Đông Bắc Việt Nam, là một điểm đến lý tưởng
            kết hợp giữa thiên nhiên hùng vĩ và di sản văn hóa phong phú. Được
            mệnh danh là “thủ phủ chè” của Việt Nam, Thái Nguyên nổi tiếng với
            những đồi chè Tân Cương xanh mướt, nơi sản xuất loại trà thơm ngon
            nhất cả nước.
          </p>
          <p>
            Không chỉ có cảnh quan thiên nhiên, Thái Nguyên còn là nơi lưu giữ
            những giá trị lịch sử và văn hóa đặc sắc. Từ các di tích cách mạng
            như ATK Định Hóa đến các ngôi chùa cổ kính như Chùa Hang, du khách
            sẽ được đắm mình trong không gian lịch sử và tâm linh. Với hơn 30
            dân tộc anh em sinh sống, Thái Nguyên mang đến một bức tranh văn hóa
            đa dạng với các lễ hội truyền thống, trang phục thổ cẩm và ẩm thực
            độc đáo.
          </p>
        </div>
        <div>
          <img
            src="https://toquoc.mediacdn.vn/Uploaded/minhkhanh/2018_08_19/chef/BINM4708_AOAV.jpg"
            alt="Đồi chè Tân Cương"
            class="w-full h-auto rounded-xl shadow-lg transform hover:scale-105 transition duration-500"
          />
        </div>
      </div>
    </section>

    <!-- Highlights -->
    <section class="mb-16">
      <h2
        class="text-3xl md:text-4xl font-bold text-green-800 mb-6 text-center"
      >
        Điểm Nổi Bật
      </h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div
          class="bg-white/95 backdrop-blur-sm p-6 rounded-xl shadow-lg hover:shadow-xl transition duration-300"
        >
          <h3 class="text-xl font-semibold text-gray-800 mb-2">
            Đồi Chè Tân Cương
          </h3>
          <p class="text-gray-600">
            Thưởng thức không khí trong lành và hương thơm của những cánh đồng
            chè xanh mướt, nơi sản xuất trà Tân Cương nổi tiếng.
          </p>
        </div>
        <div
          class="bg-white/95 backdrop-blur-sm p-6 rounded-xl shadow-lg hover:shadow-xl transition duration-300"
        >
          <h3 class="text-xl font-semibold text-gray-800 mb-2">Hồ Núi Cốc</h3>
          <p class="text-gray-600">
            Một hồ nước thơ mộng với truyền thuyết tình yêu lãng mạn, lý tưởng
            cho các chuyến dã ngoại và ngắm cảnh.
          </p>
        </div>
        <div
          class="bg-white/95 backdrop-blur-sm p-6 rounded-xl shadow-lg hover:shadow-xl transition duration-300"
        >
          <h3 class="text-xl font-semibold text-gray-800 mb-2">ATK Định Hóa</h3>
          <p class="text-gray-600">
            Khám phá khu di tích lịch sử, nơi ghi dấu những năm tháng hào hùng
            của cách mạng Việt Nam.
          </p>
        </div>
      </div>
    </section>

    <!-- Culture and Festivals -->
    <section class="mb-16">
      <h2
        class="text-3xl md:text-4xl font-bold text-green-800 mb-6 text-center"
      >
        Văn Hóa và Lễ Hội
      </h2>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
        <div>
          <img
            src="https://khamphadisan.com.vn/wp-content/uploads/2016/05/ho-nui-coc-4.jpg"
            alt="Lễ hội Thái Nguyên"
            class="w-full h-auto rounded-xl shadow-lg transform hover:scale-105 transition duration-500"
          />
        </div>
        <div class="text-gray-700 leading-relaxed">
          <p class="mb-4">
            Thái Nguyên là nơi hội tụ văn hóa của hơn 30 dân tộc, nổi bật là dân
            tộc Tày, Nùng, và Thái. Các lễ hội truyền thống như Lễ hội Lồng Tồng
            (cầu mùa), Lễ hội Đền Đuổm, hay Lễ hội Rước Nước mang đậm nét văn
            hóa tâm linh và cộng đồng.
          </p>
          <p>
            Du khách có thể trải nghiệm các điệu múa Then, hát Sli, Sình Ca,
            hoặc thưởng thức trang phục thổ cẩm rực rỡ. Các phiên chợ vùng cao
            như Chợ Đồn, Phú Lương cũng là nơi tuyệt vời để khám phá đời sống
            văn hóa và mua sắm sản phẩm thủ công.
          </p>
        </div>
      </div>
    </section>

    <!-- Cuisine -->
    <section class="mb-16">
      <h2
        class="text-3xl md:text-4xl font-bold text-green-800 mb-6 text-center"
      >
        Ẩm Thực Đặc Sắc
      </h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div
          class="bg-white/95 backdrop-blur-sm p-6 rounded-xl shadow-lg hover:shadow-xl transition duration-300"
        >
          <h3 class="text-xl font-semibold text-gray-800 mb-2">Cơm Lam</h3>
          <p class="text-gray-600">
            Cơm nếp thơm ngon được nướng trong ống tre, thường ăn kèm muối vừng
            hoặc thịt nướng.
          </p>
        </div>
        <div
          class="bg-white/95 backdrop-blur-sm p-6 rounded-xl shadow-lg hover:shadow-xl transition duration-300"
        >
          <h3 class="text-xl font-semibold text-gray-800 mb-2">
            Bánh Chưng Bờ Đậu
          </h3>
          <p class="text-gray-600">
            Loại bánh chưng đặc sản với nhân đậu xanh béo ngậy, gói bằng lá dong
            rừng.
          </p>
        </div>
        <div
          class="bg-white/95 backdrop-blur-sm p-6 rounded-xl shadow-lg hover:shadow-xl transition duration-300"
        >
          <h3 class="text-xl font-semibold text-gray-800 mb-2">Xôi Trám</h3>
          <p class="text-gray-600">
            Món xôi độc đáo với nhân trám rừng chua ngọt, mang hương vị đặc
            trưng của vùng cao.
          </p>
        </div>
      </div>
    </section>

    <!-- Image Gallery -->
    <section class="mb-16">
      <h2
        class="text-3xl md:text-4xl font-bold text-green-800 mb-6 text-center"
      >
        Khám Phá Qua Hình Ảnh
      </h2>
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <img
          src="https://toquoc.mediacdn.vn/Uploaded/minhkhanh/2018_08_19/chef/BINM4708_AOAV.jpg"
          alt="Đồi chè Tân Cương"
          class="w-full h-64 object-cover rounded-xl shadow-lg hover:scale-105 transition duration-500"
        />
        <img
          src="https://123tadi.com/wp-content/Content/images/khek0.jpg"
          alt="Thác nước Khe Cả"
          class="w-full h-64 object-cover rounded-xl shadow-lg hover:scale-105 transition duration-500"
        />
        <img
          src="https://khamphadisan.com.vn/wp-content/uploads/2016/05/ho-nui-coc-4.jpg"
          alt="Hồ Núi Cốc"
          class="w-full h-64 object-cover rounded-xl shadow-lg hover:scale-105 transition duration-500"
        />
        <img
          src="https://lophocnguvan.com/wp-content/uploads/thuyet-minh-ve-chua-hang-kim-son-tu-tinh-thai-nguyen.png"
          alt="Chùa Hang"
          class="w-full h-64 object-cover rounded-xl shadow-lg hover:scale-105 transition duration-500"
        />
      </div>
    </section>

    <!-- Travel Tips -->
    <section class="mb-16">
      <h2
        class="text-3xl md:text-4xl font-bold text-green-800 mb-6 text-center"
      >
        Mẹo Du Lịch
      </h2>
      <ul class="list-disc list-inside text-gray-700 max-w-3xl mx-auto">
        <li class="mb-2">
          <strong>Thời điểm lý tưởng:</strong> Tháng 3-5 và 9-11 khi thời tiết
          mát mẻ, thích hợp cho tham quan và chụp ảnh.
        </li>
        <li class="mb-2">
          <strong>Di chuyển:</strong> Thuê xe máy để dễ dàng khám phá các vùng
          ngoại ô và đồi chè.
        </li>
        <li class="mb-2">
          <strong>Trang phục:</strong> Mang giày thoải mái và áo khoác nhẹ cho
          các chuyến đi rừng hoặc thác nước.
        </li>
        <li>
          <strong>Đặc sản mang về:</strong> Mua chè Tân Cương, măng rừng, hoặc
          mật ong làm quà tặng.
        </li>
      </ul>
    </section>

    <!-- Call to Action -->
    <section class="text-center py-12 bg-green-50 rounded-xl">
      <h2 class="text-3xl font-bold text-green-800 mb-4">
        Sẵn Sàng Khám Phá Thái Nguyên?
      </h2>
      <p class="text-gray-600 mb-6 max-w-xl mx-auto">
        Đăng ký ngay hôm nay để nhận thông tin mới nhất về các điểm đến, lễ hội,
        và ưu đãi du lịch tại Thái Nguyên!
      </p>
      <router-link
        to="/register"
        class="inline-block bg-green-600 hover:bg-green-700 text-white font-semibold py-3 px-8 rounded-lg transition duration-300"
      >
        Đăng Ký Ngay
      </router-link>
    </section>
  </div>

  <FooterComponent />
</template>

<script setup>
import NavbarComponentV1 from "@/components/NavbarComponentV1.vue";
import FooterComponent from "@/components/FooterComponent.vue";
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

.animate-fadeIn {
  animation: fadeIn 0.8s ease-out;
}

.delay-200 {
  animation-delay: 0.2s;
}

.delay-400 {
  animation-delay: 0.4s;
}

/* Responsive Images */
.h-64 {
  height: 16rem;
}

/* Responsive Design */
@media (max-width: 768px) {
  .text-5xl {
    font-size: 2.5rem;
  }
  .text-6xl {
    font-size: 3rem;
  }
  .p-12 {
    padding: 2rem;
  }
}
</style>
