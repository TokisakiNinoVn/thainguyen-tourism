<template>
  <div class="hero-section">
    <div class="overlay">
      <h1 class="title">
        Discover the Beauty of <span class="highlight">Thái Nguyên</span>
        <br />
        <!-- <span class="highlight"></span> -->
      </h1>
      <p class="subtitle">
        Khám phá những cảnh quan ngoạn mục, di sản văn hóa phong phú và những
        trải nghiệm đích thực trong viên đá quý phía bắc của Việt Nam.
      </p>
      <div class="search-box">
        <input
          type="text"
          placeholder="Search destinations, activities, or blogs..."
        />
        <button>
          <i class="fas fa-search"></i>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "DiscoverThaiNguyen",
};
</script>

<style scoped>
.hero-section {
  position: relative;
  height: 100vh;
  background: url("../assets/images/image-banner.jpg") center center / cover
    no-repeat;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
}

.overlay {
  background-color: rgba(0, 0, 0, 0.4);
  padding: 20px;
  color: white;
  max-width: 900px;
  width: 100%;
  border-radius: 20px;
}

.title {
  font-size: 2.5rem;
  font-weight: bold;
  color: white;
}

.highlight {
  color: #ffd700; /* gold */
}

.subtitle {
  font-size: 1.2rem;
  margin-top: 1rem;
  margin-bottom: 2rem;
  color: #e0e0e0;
}

.search-box {
  display: flex;
  justify-content: center;
  align-items: center;
  max-width: 800px;
  margin: 0 auto;
  border-radius: 30px;
  background-color: white;
  padding: 10px 20px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
}

.search-box input {
  border: none;
  outline: none;
  flex: 1;
  padding: 12px;
  font-size: 1rem;
  border-radius: 30px;
}

.search-box button {
  background-color: #28a745;
  color: white;
  border: none;
  padding: 12px 15px;
  border-radius: 50%;
  cursor: pointer;
  font-size: 1.2rem;
}

.search-box button:hover {
  background-color: #218838;
}
</style>
