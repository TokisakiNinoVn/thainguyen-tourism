<template>
  <div class="container max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
    <!-- <h2 class="text-3xl font-bold text-gray-800 mb-12 text-center">
      Explore Our Blogs
    </h2> -->

    <!-- No Results -->
    <div
      v-if="!hasBlogs"
      class="no-results flex flex-col items-center justify-center py-16"
    >
      <img
        src="../../assets/images/image-notfound.png"
        alt="No Results"
        class="w-1/3 max-w-xs mb-6"
      />
      <p class="text-lg text-gray-600">No blog posts found.</p>
    </div>

    <!-- Blog Categories -->
    <div v-else class="space-y-16">
      <div
        v-for="category in groupedBlogs"
        :key="category.id"
        class="category-section"
      >
        <div class="flex items-center justify-between mb-6">
          <h3 class="text-2xl font-semibold text-gray-700">
            {{ category.name }}
          </h3>
          <!-- <router-link
            :to="`/category/${category.id}`"
            class="text-indigo-600 hover:text-indigo-800 text-sm font-medium transition-colors duration-200"
          > -->
          <router-link
            :to="`/blogs`"
            class="text-indigo-600 hover:text-indigo-800 text-sm font-medium transition-colors duration-200"
          >
            View All
          </router-link>
        </div>
        <div class="flex justify-center">
          <div
            class="grid grid-cols-1 sm:grid-cols-3 gap-6 auto-rows-fr max-w-[1140px]"
          >
            <div
              v-for="blog in category.blogs.slice(0, 6)"
              :key="blog.Id"
              class="blog-card bg-white rounded-xl shadow-md overflow-hidden transition-all duration-300 hover:shadow-xl hover:-translate-y-1"
              style="width: 356px; height: 372px"
            >
              <router-link
                :to="`/blog/detail/${blog.Id}`"
                class="block h-full flex-col"
              >
                <div class="image-wrapper flex-shrink-0">
                  <img
                    v-if="blog.MediaList?.length > 0"
                    :src="
                      instance.defaults.baseURL + blog.MediaList[0].mediaUrl
                    "
                    :alt="blog.Title"
                    class="blog-image w-full h-45 object-cover"
                  />
                  <img
                    v-else
                    src="https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg"
                    :alt="blog.Title"
                    class="blog-image w-full h-45 object-cover"
                  />
                </div>
                <div class="p-4 flex flex-col flex-grow">
                  <div class="flex items-center text-sm text-indigo-600 mb-2">
                    <i class="fa-solid fa-location-pin mr-2"></i>
                    <span class="line-clamp-1">{{
                      blog.Location || blog.Place || "Unknown Location"
                    }}</span>
                  </div>
                  <h4
                    class="text-base font-semibold text-gray-800 mb-2 line-clamp-2 text-start"
                  >
                    {{ blog.Title }}
                  </h4>
                  <p
                    class="text-sm text-gray-600 line-clamp-2 text-start flex-grow"
                    v-html="getShortDescription(blog.Content)"
                  ></p>
                </div>
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, computed } from "vue";
import { getAllBlogApi } from "@/services/modules/blog.api";
import instance from "@/services/axiosConfig";

const listBlog = ref([]);
const categories = ref([
  { id: 1, name: "Cơ sở lưu trú khách sạn" },
  { id: 2, name: "Khu di tích lịch sử" },
  { id: 3, name: "Hợp tác xã chè" },
]);

// Group blogs by category
const groupedBlogs = computed(() => {
  const grouped = categories.value.map((category) => ({
    ...category,
    blogs: listBlog.value
      .filter(
        (blog) =>
          blog.Category === category.id || blog.categoryId === category.id
      )
      .slice(0, 6), // Limit to 6 blogs per category
  }));
  return grouped.filter((category) => category.blogs.length > 0); // Only show categories with blogs
});

// Check if there are any blogs
const hasBlogs = computed(() => {
  return groupedBlogs.value.some((category) => category.blogs.length > 0);
});

// Fetch blog list
const getListPlace = async () => {
  try {
    const response = await getAllBlogApi();
    listBlog.value = JSON.parse(response.data.data).map((blog) => ({
      ...blog,
      Category: blog.Category || blog.categoryId, // Normalize category key
    }));
  } catch (error) {
    console.error("Error fetching blogs:", error);
  }
};

// Get short description
const getShortDescription = (htmlDescription) => {
  if (!htmlDescription) return "";
  const tempEl = document.createElement("div");
  tempEl.innerHTML = htmlDescription;
  const text = tempEl.textContent || tempEl.innerText || "";
  return text.length > 80 ? text.slice(0, 80) + "..." : text;
};

onMounted(() => {
  getListPlace();
});
</script>

<style scoped>
.blog-image {
  transition: transform 0.5s ease-in-out;
}

.blog-card:hover .blog-image {
  transform: scale(1.1);
}

.blog-card {
  transition: transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out;
}
</style>
