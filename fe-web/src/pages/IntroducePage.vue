<template>
  <NavbarComponentV1 />
  
  <div class="container mx-auto p-4">
    <header class="text-center mb-8">
      <h1 class="text-4xl font-bold text-gray-800">Du lịch Thái Nguyên thông minh</h1>
      <p class="mt-4 text-lg text-gray-600">Khám phá vẻ đẹp thiên nhiên và văn hóa độc đáo của Thái Nguyên.</p>
    </header>

    <section class="mb-8">
      <h2 class="text-2xl font-semibold text-gray-700 mb-4">Giới thiệu về Thái Nguyên</h2>
      <p class="text-gray-600">
        Thái Nguyên là một tỉnh nằm ở miền Bắc Việt Nam, nổi tiếng với những cảnh đẹp thiên nhiên hùng vĩ, những đồi chè xanh mướt và nền văn hóa phong phú. Đây là điểm đến lý tưởng cho những ai yêu thích khám phá và trải nghiệm.
      </p>
    </section>

    <section class="mb-8">
      <h2 class="text-2xl font-semibold text-gray-700 mb-4">Điểm nổi bật</h2>
      <ul class="list-disc list-inside text-gray-600">
        <li>Đồi chè Tân Cương - Nơi sản xuất trà nổi tiếng.</li>
        <li>Thác nước Khe Cả - Cảnh sắc tuyệt đẹp giữa thiên nhiên.</li>
        <li>Chùa Hang - Di tích lịch sử với kiến trúc độc đáo.</li>
        <li>Ẩm thực đặc sản - Trải nghiệm các món ăn truyền thống hấp dẫn.</li>
      </ul>
    </section>

    <section class="mb-8">
      <h2 class="text-2xl font-semibold text-gray-700 mb-4">Hình ảnh</h2>
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        <img src="path/to/image1.jpg" alt="Đồi chè Tân Cương" class="w-full h-auto rounded shadow-lg" />
        <img src="path/to/image2.jpg" alt="Thác nước Khe Cả" class="w-full h-auto rounded shadow-lg" />
        <img src="path/to/image3.jpg" alt="Chùa Hang" class="w-full h-auto rounded shadow-lg" />
      </div>
    </section>
  </div>

  <FooterComponent />
</template>

<script setup>
  import NavbarComponentV1 from '@/components/NavbarComponentV1.vue';
  import FooterComponent from '@/components/FooterComponent.vue';
</script>

<style scoped>
/* Bạn có thể thêm CSS tùy chỉnh ở đây nếu cần */
</style>
