<template>
  <NavbarComponentV1 />
  <div class="create-blog max-w-4xl mx-auto p-6 bg-white rounded-xl shadow-lg">
    <h1 class="text-3xl font-bold text-gray-800 mb-8 text-center">
      Tạo Bài Viết Mới
    </h1>
    <form @submit.prevent="submitForm" class="space-y-6">
      <!-- Thumbnail Input -->
      <div class="form-group">
        <label
          for="thumbnail"
          class="block text-sm font-medium text-gray-700 mb-2"
          >Ảnh đại diện</label
        >
        <input
          type="file"
          id="thumbnail"
          @change="onThumbnailChange"
          accept="image/*"
          required
          class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100"
        />
        <div v-if="thumbnailPreview" class="mt-4">
          <img
            :src="thumbnailPreview"
            alt="Xem trước ảnh đại diện"
            class="w-48 h-48 object-cover rounded-lg shadow-sm"
          />
        </div>
      </div>

      <!-- Place Input -->
      <div class="form-group">
        <label for="place" class="block text-sm font-medium text-gray-700 mb-2"
          >Địa điểm</label
        >
        <input
          type="text"
          id="place"
          v-model="blog.place"
          required
          class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
          placeholder="Nhập địa điểm..."
        />
      </div>

      <!-- Title Input -->
      <div class="form-group">
        <label for="title" class="block text-sm font-medium text-gray-700 mb-2"
          >Tiêu đề</label
        >
        <input
          type="text"
          id="title"
          v-model="blog.title"
          required
          class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
          placeholder="Nhập tiêu đề bài viết..."
        />
      </div>

      <!-- Category Selection -->
      <div class="form-group">
        <label
          for="categoryId"
          class="block text-sm font-medium text-gray-700 mb-2"
          >Danh mục</label
        >
        <select
          v-model="blog.categoryId"
          id="categoryId"
          required
          class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
        >
          <option value="" disabled>Chọn danh mục...</option>
          <option
            v-for="category in categories"
            :key="category.id"
            :value="category.id"
          >
            {{ category.name }}
          </option>
        </select>
      </div>

      <!-- Content Editor -->
      <div class="form-group">
        <label class="block text-sm font-medium text-gray-700 mb-2"
          >Mô tả</label
        >
        <quill-editor
          v-model:content="blog.content"
          content-type="html"
          theme="snow"
          :options="editorOptions"
          class="editor rounded-lg shadow-sm bg-white"
        />
        <p class="mt-2 text-sm text-gray-500">
          {{ descriptionLength }}/2000 ký tự
        </p>
      </div>

      <!-- Media Input -->
      <div class="form-group">
        <label for="media" class="block text-sm font-medium text-gray-700 mb-2"
          >Tệp phương tiện (tối đa 5 tệp)</label
        >
        <input
          type="file"
          id="media"
          @change="onMediaChange"
          accept="image/*,video/*"
          multiple
          class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100"
        />
        <div
          class="mt-4 grid grid-cols-2 sm:grid-cols-3 gap-4"
          v-if="mediaPreviews.length"
        >
          <div
            v-for="(preview, index) in mediaPreviews"
            :key="index"
            class="relative"
          >
            <img
              v-if="preview.type === 'image'"
              :src="preview.url"
              alt="Xem trước tệp phương tiện"
              class="w-full h-32 object-cover rounded-lg shadow-sm"
            />
            <video
              v-else
              :src="preview.url"
              controls
              class="w-full h-32 object-cover rounded-lg shadow-sm"
            ></video>
            <button
              type="button"
              @click="removeMedia(index)"
              class="absolute -top-2 -right-2 bg-red-500 text-white rounded-full w-6 h-6 flex items-center justify-center hover:bg-red-600 transition"
            >
              ×
            </button>
          </div>
        </div>
        <p v-if="mediaPreviews.length >= 5" class="mt-2 text-sm text-red-600">
          Chỉ được phép tối đa 5 tệp.
        </p>
      </div>

      <button
        type="submit"
        :disabled="isSubmitting || descriptionLength > 2000"
        class="w-full py-3 px-4 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition disabled:bg-gray-400 disabled:cursor-not-allowed flex items-center justify-center"
      >
        <svg
          v-if="isSubmitting"
          class="animate-spin -ml-1 mr-3 h-5 w-5 text-white"
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
        >
          <circle
            class="opacity-25"
            cx="12"
            cy="12"
            r="10"
            stroke="currentColor"
            stroke-width="4"
          ></circle>
          <path
            class="opacity-75"
            fill="currentColor"
            d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
          ></path>
        </svg>
        {{ isSubmitting ? "Đang tạo..." : "Tạo bài viết" }}
      </button>
    </form>
  </div>
  <FooterComponent />
</template>

<script setup>
import { ref, computed } from "vue";
import "quill/dist/quill.snow.css";
import { QuillEditor } from "@vueup/vue-quill";
import {
  postCreatBlogApi,
  uploadSingleFileApi,
} from "@/services/modules/blog.api";
import { useRouter } from "vue-router";
import NavbarComponentV1 from "@/components/NavbarComponentV1.vue";
import FooterComponent from "@/components/FooterComponent.vue";

// Blog data
const blog = ref({
  title: "",
  content: "",
  place: "",
  categoryId: "",
});

// Thumbnail and media handling
const thumbnailFile = ref(null);
const thumbnailPreview = ref(null);
const mediaFiles = ref([]);
const mediaPreviews = ref([]);
const isSubmitting = ref(false);
const router = useRouter();

const categories = ref([
  { id: 1, name: "Cơ sở lưu trú khách sạn" },
  { id: 2, name: "Khu di tích lịch sử" },
  { id: 3, name: "Hợp tác xã chè" },
]);

// Quill editor options
const editorOptions = {
  placeholder: "Write Mississauga: Write your blog content here...",
  modules: {
    toolbar: [
      ["bold", "italic", "underline", "strike"],
      ["blockquote", "code-block"],
      [{ header: 1 }, { header: 2 }],
      [{ list: "ordered" }, { list: "bullet" }],
      [{ indent: "-1" }, { indent: "+1" }],
      ["link", "image"],
      ["clean"],
    ],
  },
};

// Compute character length for content
const descriptionLength = computed(() => {
  const text = blog.value.content.replace(/<[^>]*>/g, "");
  return text.length;
});

// Sanitize input
const sanitizeInput = (input) => {
  return input.replace(/[<>]/g, "");
};

// Handle thumbnail file change
const onThumbnailChange = (event) => {
  const file = event.target.files[0];
  if (file) {
    if (!file.type.startsWith("image/")) {
      alert("Please upload a valid image file.");
      event.target.value = "";
      return;
    }
    thumbnailFile.value = file;
    const reader = new FileReader();
    reader.onload = (e) => {
      thumbnailPreview.value = e.target.result;
    };
    reader.readAsDataURL(file);
  } else {
    thumbnailFile.value = null;
    thumbnailPreview.value = null;
  }
};

// Handle media file change
const onMediaChange = (event) => {
  const files = Array.from(event.target.files);
  if (mediaFiles.value.length + files.length > 5) {
    alert("You can upload a maximum of 5 media files.");
    event.target.value = "";
    return;
  }

  files.forEach((file) => {
    if (file.type.startsWith("image/") || file.type.startsWith("video/")) {
      mediaFiles.value.push(file);
      const reader = new FileReader();
      reader.onload = (e) => {
        mediaPreviews.value.push({
          url: e.target.result,
          type: file.type.startsWith("image/") ? "image" : "video",
        });
      };
      reader.readAsDataURL(file);
    }
  });
  event.target.value = "";
};

// Remove media file
const removeMedia = (index) => {
  mediaFiles.value.splice(index, 1);
  mediaPreviews.value.splice(index, 1);
};

// Submit form
const submitForm = async () => {
  if (descriptionLength.value > 2000) {
    alert("Content exceeds 2000 characters.");
    return;
  }

  if (!thumbnailFile.value) {
    alert("Please upload a thumbnail.");
    return;
  }

  if (!blog.value.categoryId) {
    alert("Please select a category.");
    return;
  }

  isSubmitting.value = true;

  confirm(
    "Bạn có chắc chắn muốn tạo và phát hành bài viết này không? Các nội dung bài viết sẽ không thể thay đổi"
  )
    ? null
    : (isSubmitting.value = false);
  if (!isSubmitting.value) return;

  try {
    // Sanitize inputs
    const sanitizedBlogData = {
      title: sanitizeInput(blog.value.title),
      content: blog.value.content,
      place: sanitizeInput(blog.value.place),
      categoryId: parseInt(blog.value.categoryId),
    };

    // Create blog
    const response = await postCreatBlogApi(sanitizedBlogData);
    const blogId = response.data.data.id;

    // Upload thumbnail
    const thumbnailFormData = new FormData();
    thumbnailFormData.append("BlogId", blogId);
    thumbnailFormData.append("Type", 1);
    thumbnailFormData.append("ImageFor", "thumbnail");
    thumbnailFormData.append("File", thumbnailFile.value);

    await uploadSingleFileApi(thumbnailFormData);

    // Upload media files
    for (const file of mediaFiles.value) {
      const mediaFormData = new FormData();
      mediaFormData.append("BlogId", blogId);
      mediaFormData.append("Type", file.type.startsWith("image/") ? 1 : 2);
      mediaFormData.append("ImageFor", "media");
      mediaFormData.append("File", file);

      await uploadSingleFileApi(mediaFormData);
    }

    alert("Blog created successfully!");
    router.push("/");
  } catch (error) {
    console.error("Error creating blog:", error);
    alert(
      error.response?.data?.message ||
        "An error occurred while creating the blog."
    );
  } finally {
    isSubmitting.value = false;
  }
};
</script>

<style scoped>
/* Tailwind CSS is used directly in the template, so no additional scoped styles are needed */
</style>
