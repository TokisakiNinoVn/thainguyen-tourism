<template>
  <div class="container max-w-5xl mx-auto p-8 bg-gradient-to-br from-gray-50 to-white rounded-2xl shadow-xl transition-all duration-300">
    <header class="mb-8">
      <h2 class="text-3xl font-extrabold text-white tracking-tight">Cập nhật thông tin địa điểm</h2>
      <p class="mt-2 text-sm text-white">Cập nhật chi tiết địa điểm và ảnh đại diện một cách dễ dàng</p>
    </header>

    <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
      <!-- Form thông tin địa điểm -->
      <div class="space-y-8">
        <section>
          <h3 class="text-xl font-semibold text-gray-800 mb-6 border-b pb-2 border-gray-200">Thông tin chi tiết</h3>
          <div class="space-y-6">
            <div class="form-group">
              <label class="label">Vĩ độ</label>
              <input
                v-model="newLocation.latitude"
                type="number"
                step="any"
                placeholder="Nhập vĩ độ"
                class="input"
              />
            </div>
            <div class="form-group">
              <label class="label">Kinh độ</label>
              <input
                v-model="newLocation.longitude"
                type="number"
                step="any"
                placeholder="Nhập kinh độ"
                class="input"
              />
            </div>
            <div class="form-group">
              <label class="label">Tên địa điểm <span class="text-red-500">*</span></label>
              <input
                v-model="newLocation.name"
                type="text"
                placeholder="Nhập tên địa điểm"
                required
                class="input"
              />
            </div>
            <div class="form-group">
              <label class="label">Mô tả</label>
              <quill-editor
                v-model:content="newLocation.description"
                content-type="html"
                theme="snow"
                :options="editorOptions"
                class="editor rounded-lg shadow-sm"
                @update:content="checkDescriptionLength"
              />
              <p class="mt-2 text-sm text-gray-500">
                {{ descriptionLength }}/2000 ký tự
              </p>
            </div>
            <div class="form-group">
              <label class="label">Đường dẫn (Đường dẫn tới trang giới thiệu địa điểm)</label>
              <input
                v-model="newLocation.url"
                type="url"
                placeholder="Nhập URL"
                class="input"
              />
            </div>
          </div>
        </section>
        <div class="flex gap-4">
          <button
            @click="saveLocation"
            class="flex-1 btn btn-primary"
          >
            <i class="fas fa-save mr-2"></i>Lưu thay đổi
          </button>
          <button
            @click="cancelForm"
            class="flex-1 btn btn-secondary"
          >
            <i class="fas fa-times mr-2"></i>Hủy
          </button>
        </div>
      </div>

      <!-- Form thumbnail -->
      <div class="space-y-8">
        <section>
          <h3 class="text-xl font-semibold text-gray-800 mb-6 border-b pb-2 border-gray-200">Ảnh đại diện</h3>
          <div class="space-y-6">
            <div class="form-group">
              <label class="label">Chọn ảnh mới</label>
              <div class="relative">
                <input
                  id="thumbnail"
                  type="file"
                  accept="image/*"
                  @change="handleThumbnailChange"
                  class="input-file"
                />
                <span class="absolute inset-y-0 left-0 flex items-center pl-3 text-gray-500">
                  <i class="fas fa-image"></i>
                </span>
              </div>
            </div>
            <div v-if="thumbnail" class="thumbnail-preview">
              <h4 class="text-sm font-medium text-gray-600 mb-2">Xem trước</h4>
              <div class="relative group">
                <img
                  :src="thumbnail"
                  alt="Thumbnail Preview"
                  class="w-full h-64 object-cover rounded-xl shadow-md transition-transform duration-300 group-hover:scale-105"
                />
                <div class="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-20 transition-opacity duration-300 rounded-xl"></div>
              </div>
            </div>
            <button
              @click="updateThumbnail"
              :disabled="!newThumbnailFile"
              class="w-full btn btn-success"
            >
              <i class="fas fa-upload mr-2"></i>Tải ảnh lên
            </button>
          </div>
        </section>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { getPlaceByIdApi, updatePlaceApi, updateThumbnailApi } from '@/apis/modules/place.api';
import { uploadSingleFileApi } from '@/apis/modules/upload.api';
import instance from '@/apis/axiosConfig';
import { QuillEditor } from '@vueup/vue-quill';
import 'quill/dist/quill.snow.css';

const route = useRoute();
const placeId = route.params.id;

const newLocation = ref({
  latitude: 0,
  longitude: 0,
  name: '',
  description: '',
  url: '',
  imageUrl: null,
});

const thumbnail = ref(null);
const newThumbnailFile = ref(null);
const descriptionLength = ref(0);

const editorOptions = {
  theme: 'snow',
  modules: {
    toolbar: [
      ['bold', 'italic', 'underline', 'strike'],
      ['link', 'image'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ header: [1, 2, 3, false] }],
      ['clean'],
    ],
  },
};

const checkDescriptionLength = () => {
  const text = newLocation.value.description.replace(/<[^>]*>/g, '');
  descriptionLength.value = text.length;
  if (text.length > 2000) {
    newLocation.value.description = text.slice(0, 2000);
    alert('Mô tả không được vượt quá 2000 ký tự!');
  }
};

const fetchPlace = async () => {
  try {
    const response = await getPlaceByIdApi(placeId);
    if (response.code === 200) {
      newLocation.value = {
        latitude: response.data.latitude || 0,
        longitude: response.data.longitude || 0,
        name: response.data.name || '',
        description: response.data.description || '',
        url: response.data.url || '',
        imageUrl: response.data.imageUrl || null,
      };
      thumbnail.value = response.data.imageUrl
        ? instance.defaults.baseURL + response.data.imageUrl
        : null;
      checkDescriptionLength();
    }
  } catch (error) {
    console.error('Lỗi khi lấy thông tin địa điểm:', error);
    alert('Không thể tải thông tin địa điểm.');
  }
};

const handleThumbnailChange = (event) => {
  const file = event.target.files[0];
  if (!file) {
    thumbnail.value = null;
    newThumbnailFile.value = null;
    return;
  }

  const validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];
  if (!validImageTypes.includes(file.type)) {
    alert('Vui lòng chọn một tệp hình ảnh hợp lệ (JPEG, PNG, GIF).');
    return;
  }

  newThumbnailFile.value = file;
  const reader = new FileReader();
  reader.onload = (e) => {
    thumbnail.value = e.target.result;
  };
  reader.onerror = (error) => {
    console.error('Lỗi khi đọc file:', error);
    alert('Không thể đọc tệp hình ảnh. Vui lòng thử lại.');
  };
  reader.readAsDataURL(file);
};

const updateThumbnail = async () => {
  if (!newThumbnailFile.value) {
    alert('Vui lòng chọn ảnh để cập nhật.');
    return;
  }

  try {
    const formData = new FormData();
    formData.append('file', newThumbnailFile.value);
    formData.append('type', '1');
    formData.append('imagefor', 'thumbnail');
    formData.append('placeId', placeId);

    const uploadResponse = await uploadSingleFileApi(formData);
    if (!uploadResponse?.data?.id) {
      throw new Error('Không nhận được URL từ phản hồi upload.');
    }

    const thumbnailId = uploadResponse.data.id;
    await updateThumbnailApi(placeId, thumbnailId);

    thumbnail.value = instance.defaults.baseURL + uploadResponse.data.url;
    newThumbnailFile.value = null;
    alert('Cập nhật ảnh đại diện thành công!');
  } catch (error) {
    console.error('Lỗi khi cập nhật thumbnail:', error);
    alert('Không thể cập nhật ảnh đại diện. Vui lòng thử lại.');
  }
};

const saveLocation = async () => {
  if (!newLocation.value.name) {
    alert('Vui lòng nhập tên địa điểm.');
    return;
  }
  try {
    const data = { ...newLocation.value };
    await updatePlaceApi(placeId, data);
    alert('Cập nhật thông tin địa điểm thành công!');
  } catch (error) {
    console.error('Lỗi khi cập nhật địa điểm:', error);
    alert('Không thể cập nhật địa điểm.');
  }
};

const cancelForm = () => {
  fetchPlace();
  newThumbnailFile.value = null;
};

onMounted(() => {
  fetchPlace();
});
</script>

<style scoped>
.container {
  animation: fadeIn 0.5s ease-in-out;
}

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

.label {
  @apply block text-sm font-medium text-gray-700 mb-1.5 transition-colors duration-200;
}

.input {
  @apply w-full px-4 py-3 border border-gray-300 rounded-xl shadow-sm focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-all duration-200;
}

.input:focus {
  box-shadow: 0 0 8px rgba(99, 102, 241, 0.2);
}

.input-file {
  @apply w-full px-10 py-3 border border-gray-300 rounded-xl shadow-sm cursor-pointer file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100 transition-all duration-200;
}

.btn {
  @apply px-6 py-3 rounded-xl font-semibold shadow-sm transition-all duration-200 transform hover:-translate-y-0.5;
}

.btn-primary {
  @apply bg-indigo-600 text-white hover:bg-indigo-700;
}

.btn-secondary {
  @apply bg-gray-200 text-gray-700 hover:bg-gray-300;
}

.btn-success {
  @apply bg-green-600 text-white hover:bg-green-700 disabled:bg-gray-300 disabled:cursor-not-allowed disabled:shadow-none disabled:transform-none;
}

.editor :deep(.ql-container) {
  min-height: 200px;
  border-radius: 0 0 12px 12px;
  border: 1px solid #e5e7eb;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
}

.editor :deep(.ql-toolbar) {
  border-radius: 12px 12px 0 0;
  border: 1px solid #e5e7eb;
  border-bottom: none;
  background: #f9fafb;
}

.form-group {
  animation: slideIn 0.3s ease-in-out;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(-10px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.thumbnail-preview img {
  transition: all 0.3s ease-in-out;
}
</style>