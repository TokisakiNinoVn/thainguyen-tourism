<template>
  <NavbarComponentV1 />
  <div
    class="relative min-h-screen bg-cover bg-center"
    style="
      background-image: url('https://toquoc.mediacdn.vn/Uploaded/minhkhanh/2018_08_19/chef/BINM4708_AOAV.jpg');
    "
  >
    <!-- Overlay for readability -->
    <div class="absolute inset-0 bg-black/50"></div>

    <!-- Form Container -->
    <div
      class="relative max-w-4xl mx-auto p-8 md:p-12 bg-white/95 backdrop-blur-sm rounded-2xl shadow-xl mt-8 mb-12 animate-fadeIn"
    >
      <h1
        class="text-4xl font-bold text-green-800 mb-10 text-center tracking-tight"
      >
        Tạo Bài Viết Du Lịch Thái Nguyên
      </h1>
      <form @submit.prevent="submitForm" class="space-y-8">
        <!-- Thumbnail Input -->
        <div class="form-group">
          <label
            for="thumbnail"
            class="block text-sm font-medium text-gray-700 mb-2"
          >
            Ảnh Đại Diện
          </label>
          <div class="relative">
            <input
              type="file"
              id="thumbnail"
              @change="onThumbnailChange"
              accept="image/*"
              required
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-3 file:px-6 file:rounded-lg file:border-0 file:text-sm file:font-semibold file:bg-green-50 file:text-green-700 hover:file:bg-green-100 transition duration-200"
            />
            <svg
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
              />
            </svg>
          </div>
          <div v-if="thumbnailPreview" class="mt-4">
            <img
              :src="thumbnailPreview"
              alt="Xem trước ảnh đại diện"
              class="w-64 h-64 object-cover rounded-xl shadow-md transform hover:scale-105 transition duration-300"
            />
          </div>
        </div>

        <!-- Place Input -->
        <div class="form-group">
          <label
            for="place"
            class="block text-sm font-medium text-gray-700 mb-2"
          >
            Địa Điểm
          </label>
          <div class="relative">
            <input
              type="text"
              id="place"
              v-model="blog.place"
              required
              class="w-full pl-10 pr-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition duration-200"
              placeholder="Ví dụ: Đồi Chè Tân Cương"
            />
            <svg
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"
              />
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"
              />
            </svg>
          </div>
        </div>

        <!-- Title Input -->
        <div class="form-group">
          <label
            for="title"
            class="block text-sm font-medium text-gray-700 mb-2"
          >
            Tiêu Đề
          </label>
          <div class="relative">
            <input
              type="text"
              id="title"
              v-model="blog.title"
              required
              class="w-full pl-10 pr-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition duration-200"
              placeholder="Ví dụ: Khám Phá Hồ Núi Cốc"
            />
            <svg
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
              />
            </svg>
          </div>
        </div>

        <!-- Category Selection -->
        <div class="form-group">
          <label
            for="category"
            class="block text-sm font-medium text-gray-700 mb-2"
          >
            Danh Mục
          </label>
          <div class="relative">
            <select
              v-model="blog.category"
              id="category"
              required
              class="w-full pl-10 pr-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition duration-200 appearance-none"
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
            <svg
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z"
              />
            </svg>
          </div>
        </div>

        <!-- Content Editor -->
        <div class="form-group">
          <label
            for="content"
            class="block text-sm font-medium text-gray-700 mb-2"
          >
            Nội Dung
          </label>
          <quill-editor
            v-model:content="blog.content"
            content-type="html"
            theme="snow"
            :options="editorOptions"
            class="editor rounded-lg shadow-md bg-white"
          />
          <div class="mt-2 flex justify-between items-center">
            <p class="text-sm text-gray-500">
              {{ descriptionLength }}/2000 ký tự
            </p>
            <p
              v-if="descriptionLength > 2000"
              class="text-sm text-red-600 animate-pulse"
            >
              Vượt quá giới hạn ký tự!
            </p>
          </div>
        </div>

        <!-- Media Input -->
        <div class="form-group">
          <label
            for="media"
            class="block text-sm font-medium text-gray-700 mb-2"
          >
            Tệp Phương Tiện (Tối đa 5)
          </label>
          <div class="relative">
            <input
              type="file"
              id="media"
              @change="onMediaChange"
              accept="image/*,video/*"
              multiple
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-3 file:px-6 file:rounded-lg file:border-0 file:text-sm file:font-semibold file:bg-green-50 file:text-green-700 hover:file:bg-green-100 transition duration-200"
            />
            <svg
              class="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z"
              />
            </svg>
          </div>
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
                class="w-full h-40 object-cover rounded-lg shadow-md hover:scale-105 transition duration-300"
              />
              <video
                v-else
                :src="preview.url"
                controls
                class="w-full h-40 object-cover rounded-lg shadow-md hover:scale-105 transition duration-300"
              ></video>
              <button
                type="button"
                @click="removeMedia(index)"
                class="absolute -top-2 -right-2 bg-red-500 text-white rounded-full w-6 h-6 flex items-center justify-center hover:bg-red-600 transition duration-200"
              >
                ×
              </button>
            </div>
          </div>
          <p
            v-if="mediaPreviews.length >= 5"
            class="mt-2 text-sm text-red-600 animate-pulse"
          >
            Đã đạt giới hạn 5 tệp!
          </p>
        </div>

        <!-- Submit Button -->
        <button
          type="submit"
          :disabled="isSubmitting || descriptionLength > 2000"
          class="w-full py-3 px-4 bg-green-600 hover:bg-green-700 text-white rounded-lg font-semibold transition duration-300 transform hover:-translate-y-1 flex items-center justify-center disabled:bg-gray-400 disabled:cursor-not-allowed disabled:transform-none"
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
          {{ isSubmitting ? "Đang Tạo..." : "Tạo Bài Viết" }}
        </button>
      </form>
    </div>
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
  category: "",
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
  placeholder: "Viết về trải nghiệm du lịch Thái Nguyên của bạn...",
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
      alert("Vui lòng tải lên tệp hình ảnh hợp lệ.");
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
    alert("Bạn chỉ có thể tải lên tối đa 5 tệp phương tiện.");
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
    alert("Nội dung vượt quá 2000 ký tự.");
    return;
  }

  if (!thumbnailFile.value) {
    alert("Vui lòng tải lên ảnh đại diện.");
    return;
  }

  if (!blog.value.category) {
    alert("Vui lòng chọn danh mục.");
    return;
  }

  isSubmitting.value = true;

  if (
    !confirm(
      "Bạn có chắc chắn muốn tạo và phát hành bài viết này không? Nội dung sẽ không thể chỉnh sửa sau khi gửi."
    )
  ) {
    isSubmitting.value = false;
    return;
  }

  try {
    // Sanitize inputs
    const sanitizedBlogData = {
      title: sanitizeInput(blog.value.title),
      content: blog.value.content,
      place: sanitizeInput(blog.value.place),
      category: parseInt(blog.value.category),
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

    alert("Tạo bài viết thành công!");
    router.push("/");
  } catch (error) {
    console.error("Lỗi khi tạo bài viết:", error);
    alert(
      error.response?.data?.message ||
        "Đã xảy ra lỗi khi tạo bài viết. Vui lòng thử lại."
    );
  } finally {
    isSubmitting.value = false;
  }
};
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

/* Quill Editor Styling */
:deep(.ql-editor) {
  min-height: 300px;
  font-size: 1rem;
  line-height: 1.5;
}

/* Remove default select arrow */
select {
  background-image: none;
}

/* Responsive Design */
@media (max-width: 640px) {
  .p-12 {
    padding: 2rem;
  }
  .w-64 {
    width: 100%;
  }
  .h-64 {
    height: auto;
  }
}
</style>
