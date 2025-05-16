<template>
    <NavbarComponentV1 />
  
    <div class="container">
      <!-- Input search by Place -->
      <div class="search-bar">
        <div class="search-input-wrapper">
          <i class="fa-solid fa-magnifying-glass search-icon"></i>
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Tìm kiếm bài viết..."
            class="search-input"
            @keyup.enter="searchAction"
          />
        </div>
        <button @click="searchAction" class="search-button">
          Tìm kiếm
        </button>
      </div>
  
      <h2 class="title">Danh sách bài viết</h2>
  
      <div v-if="displayedBlogs.length === 0" class="no-results">
        <img src="../../assets/images/image-notfound.png" alt="No Results" class="w-1/2 mx-auto mb-4" />
        Không tìm thấy bài viết nào.
      </div>
  
      <div class="blog-grid">
        <div
          v-for="blog in displayedBlogs"
          :key="blog.id"
          class="blog-card"
        >
          <router-link :to="`/blog/detail/${blog.Id}`" class="card-link">
            <div class="image-wrapper">
              <img
                v-if="blog.MediaList.length > 0"
                :src="instance.defaults.baseURL + blog.MediaList[0].mediaUrl"
                alt="Thumbnail"
                class="blog-image"
              />
              <img
                v-else
                src="https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg"
                alt="Thumbnail"
                class="blog-image"
              />
            </div>
            <div class="blog-content">
              <div class="location">
                <span class="location-icon"><i class="fa-solid fa-location-pin"></i></span>
                <span>{{ blog.Location || blog.Place }}</span>
              </div>
              <h3 class="blog-title">{{ blog.Title }}</h3>
              <p class="blog-description" v-html="getShortDescription(blog.Content)"></p>
              <!-- <div class="rating">
                <span class="star-icon">★</span>
                <span>{{ blog.Rating || '4.5' }}</span>
              </div> -->
            </div>
          </router-link>
        </div>
      </div>
    </div>
  
    <FooterComponent />
  </template>
  
  <script setup>
  import { onMounted, ref, computed } from 'vue';
  import NavbarComponentV1 from '@/components/NavbarComponentV1.vue';
  import FooterComponent from '@/components/FooterComponent.vue';
  import { getAllBlogApi } from '@/services/modules/blog.api';
  import instance from '@/services/axiosConfig';
  
  const listBlog = ref([]);
  const searchQuery = ref('');
  
  const displayedBlogs = computed(() => {
    if (!searchQuery.value) return listBlog.value;
    const query = searchQuery.value.toLowerCase();
    return listBlog.value.filter(blog => {
      return (
        blog.Title.toLowerCase().includes(query) ||
        blog.Content.toLowerCase().includes(query) ||
        (blog.Location || blog.Place)?.toLowerCase().includes(query)
      );
    });
  });
  
  const getListPlace = async () => {
    try {
      const response = await getAllBlogApi();
      listBlog.value = JSON.parse(response.data.data);
      console.log('List of places:', listBlog.value);
    } catch (error) {
      console.error('Error fetching list of places:', error);
    }
  };
  
  const getShortDescription = (htmlDescription) => {
    const tempEl = document.createElement('div');
    tempEl.innerHTML = htmlDescription;
    const text = tempEl.textContent || tempEl.innerText || '';
    return text.length > 80 ? text.slice(0, 80) + '...' : text;
  };
  
  const searchAction = () => {
    // Triggered by button click or Enter key
    console.log('Searching for:', searchQuery.value);
  };
  
  onMounted(() => {
    getListPlace();
  });
  </script>
  
  <style>
  .container {
    max-width: 1200px;
    min-height: 100vh;
    margin: 0 auto;
    padding: 32px 16px;
  }
  
  .search-bar {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 16px;
    margin-bottom: 32px;
    flex-wrap: wrap;
  }
  
  .search-input-wrapper {
    position: relative;
    flex: 1;
    max-width: 400px;
  }
  
  .search-icon {
    position: absolute;
    left: 12px;
    top: 50%;
    transform: translateY(-50%);
    color: #94a3b8;
  }
  
  .search-input {
    width: 100%;
    padding: 12px 16px 12px 40px;
    border: 2px solid #e2e8f0;
    border-radius: 12px;
    font-size: 16px;
    transition: border-color 0.3s, box-shadow 0.3s;
  }
  
  .search-input:focus {
    outline: none;
    border-color: #10b981;
    box-shadow: 0 0 0 3px rgba(16, 185, 129, 0.1);
  }
  
  .search-button {
    background-color: #10b981;
    color: white;
    padding: 12px 24px;
    border-radius: 12px;
    font-size: 16px;
    font-weight: 500;
    transition: background-color 0.3s, transform 0.1s;
  }
  
  .search-button:hover {
    background-color: #059669;
  }
  
  .search-button:active {
    transform: scale(0.98);
  }
  
  .title {
    font-size: 28px;
    font-weight: 700;
    color: #1e293b;
    margin-bottom: 32px;
    text-align: center;
  }
  
  .no-results {
    text-align: center;
    color: #64748b;
    font-size: 18px;
    padding: 20px;
    border-radius: 12px;
  }
  
  .blog-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 24px;
    padding-bottom: 24px;
  }
  
  .blog-card {
    background-color: #ffffff;
    border-radius: 16px;
    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.08);
    overflow: hidden;
    transition: transform 0.3s, box-shadow 0.3s;
  }
  
  .blog-card:hover {
    transform: translateY(-6px);
    box-shadow: 0 12px 24px rgba(0, 0, 0, 0.12);
  }
  
  .card-link {
    text-decoration: none;
    color: inherit;
    display: block;
  }
  
  .image-wrapper {
    position: relative;
    overflow: hidden;
  }
  
  .blog-image {
    width: 100%;
    height: 220px;
    object-fit: cover;
    transition: transform 0.3s;
  }
  
  .blog-card:hover .blog-image {
    transform: scale(1.05);
  }
  
  .blog-content {
    padding: 20px;
  }
  
  .location {
    display: flex;
    align-items: center;
    font-size: 14px;
    color: #10b981;
    margin-bottom: 8px;
  }
  
  .location-icon {
    margin-right: 6px;
    color: #10b981;
  }
  
  .blog-title {
    font-size: 20px;
    font-weight: 700;
    color: #1e293b;
    margin-bottom: 10px;
    line-height: 1.3;
    text-align: left;
  }
  
  .blog-description {
    font-size: 14px;
    color: #64748b;
    margin-bottom: 12px;
    line-height: 1.6;
    text-align: left;
  }
  
  .rating {
    display: flex;
    align-items: center;
    font-size: 14px;
    color: #f59e0b;
  }
  
  .star-icon {
    margin-right: 4px;
    color: #f59e0b;
  }
  
  @media (max-width: 768px) {
    .blog-grid {
      grid-template-columns: 1fr;
    }
  
    .search-bar {
      flex-direction: column;
      align-items: stretch;
    }
  
    .search-input-wrapper {
      max-width: 100%;
    }
  
    .search-button {
      margin-top: 12px;
    }
  }
  </style>