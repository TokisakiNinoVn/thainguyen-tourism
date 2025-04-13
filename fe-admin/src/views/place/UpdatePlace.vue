<template>
    <div class="card">
      <div class="card-header">
        <h5>Cập nhật thông tin địa điểm</h5>
      </div>
      <div class="card-body">
        <div class="container-fluid">
          <div class="row">
            <!-- Form cập nhật thông tin địa điểm -->
            <div class="col-md-6">
              <div class="form-section">
                <h6 class="section-title">Thông tin địa điểm</h6>
                <div class="form-group">
                  <label for="latitude">Vĩ độ</label>
                  <input
                    id="latitude"
                    v-model="newLocation.latitude"
                    type="number"
                    step="any"
                    placeholder="Vĩ độ"
  
                    class="form-control"
                  />
                </div>
                <div class="form-group">
                  <label for="longitude">Kinh độ</label>
                  <input
                    id="longitude"
                    v-model="newLocation.longitude"
                    type="number"
                    step="any"
                    placeholder="Kinh độ"
  
                    class="form-control"
                  />
                </div>
                <div class="form-group">
                  <label for="name">Tên địa điểm <span class="required">*</span></label>
                  <input
                    id="name"
                    v-model="newLocation.name"
                    type="text"
                    placeholder="Nhập tên địa điểm"
                    required
                    class="form-control"
                  />
                </div>
                <div class="form-group">
                  <label for="description">Mô tả</label>
                  <textarea
                    id="description"
                    v-model="newLocation.description"
                    placeholder="Nhập mô tả"
                    class="form-control"
                    rows="4"
                  ></textarea>
                </div>
                <div class="form-group">
                  <label for="url">Đường dẫn <span class="required">*</span></label>
                  <input
                    id="url"
                    v-model="newLocation.url"
                    type="url"
                    placeholder="Nhập URL"
                    required
                    class="form-control"
                  />
                </div>
                <div class="form-actions">
                  <button class="btn btn-success" @click="saveLocation">
                    <i class="fas fa-save"></i> Lưu thay đổi
                  </button>
                  <button class="btn btn-secondary" @click="cancelForm">
                    <i class="fas fa-times"></i> Hủy
                  </button>
                </div>
              </div>
            </div>
            <!-- Form cập nhật thumbnail -->
            <div class="col-md-6">
              <div class="form-section">
                <h6 class="section-title">Cập nhật ảnh đại diện</h6>
                <div class="form-group">
                  <label for="thumbnail">Chọn ảnh mới</label>
                  <input
                    id="thumbnail"
                    type="file"
                    accept="image/*"
                    @change="handleThumbnailChange"
                    class="form-control-file"
                  />
                </div>
                <div class="thumbnail-preview" v-if="thumbnail">
                  <h6>Ảnh xem trước</h6>
                  <img :src="thumbnail" alt="Thumbnail Preview" />
                </div>
                <div class="form-actions">
                  <button class="btn btn-primary" @click="updateThumbnail" :disabled="!newThumbnailFile">
                    <i class="fas fa-upload"></i> Cập nhật ảnh
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import { useRoute } from 'vue-router';
  import { getPlaceByIdApi, updatePlaceApi, updateThumbnailApi } from '@/apis/modules/place.api';
  import { uploadSingleFileApi } from '@/apis/modules/upload.api';
  
  // Lấy ID từ route params
  const route = useRoute();
  const placeId = route.params.id;
  
  // Dữ liệu địa điểm
  const newLocation = ref({
    latitude: 0,
    longitude: 0,
    name: '',
    description: '',
    url: '',
  });
  
  // Dữ liệu thumbnail
  const thumbnail = ref(null);
  const newThumbnailFile = ref(null); // Lưu file mới được chọn để kiểm tra trước khi cập nhật
  
  // Lấy thông tin địa điểm
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
        };
        thumbnail.value = response.data.thumbnail || null;
      }
    } catch (error) {
      console.error('Lỗi khi lấy thông tin địa điểm:', error);
      alert('Không thể tải thông tin địa điểm.');
    }
  };
  
  // Xử lý thay đổi thumbnail
const handleThumbnailChange = (event) => {
    const file = event.target.files[0];
    console.log('file', file);
    
    if (!file) {
        thumbnail.value = null; // Reset thumbnail nếu không có file
        return;
    }

    // Kiểm tra định dạng file (chỉ cho phép ảnh)
    const validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];
    if (!validImageTypes.includes(file.type)) {
        alert('Vui lòng chọn một tệp hình ảnh hợp lệ (JPEG, PNG, GIF).');
        return;
    }

    newThumbnailFile.value = file;
    const reader = new FileReader();
    
    reader.onload = (e) => {
        thumbnail.value = e.target.result; // Hiển thị preview ảnh tạm thời
    };
    
    reader.onerror = (error) => {
        console.error('Lỗi khi đọc file:', error);
        alert('Không thể đọc tệp hình ảnh. Vui lòng thử lại.');
    };
    
    reader.readAsDataURL(file);
};

// Cập nhật thumbnail (upload trước, sau đó gọi API cập nhật)
const updateThumbnail = async () => {
    if (!newThumbnailFile.value) {
        alert('Vui lòng chọn ảnh để cập nhật.');
        return;
    }

    console.log('newThumbnailFile.value', newThumbnailFile.value);
    
    try {
        // Bước 1: Upload ảnh lên server
        const formData = new FormData();
        formData.append('file', newThumbnailFile.value);
        formData.append('type', '1');
        // console.log('formData', formData);
        for (const [key, value] of formData.entries()) {
            console.log(`${key}:`, value);
        }

        let uploadResponse;

        try {
            uploadResponse = await uploadSingleFileApi(formData);
        } catch (error) {
            console.error('Lỗi khi upload ảnh:', error);
            // alert('Không thể upload ảnh đại diện. Vui lòng kiểm tra lại kết nối.');
            return;
        }

        console.log('uploadResponse', uploadResponse);

        if (!uploadResponse || !uploadResponse.data || !uploadResponse.data.id) {
            throw new Error('Không nhận được URL từ phản hồi upload.');
        }

        const thumbnailUrl = uploadResponse.data.id;

        // Bước 2: Cập nhật thumbnail cho địa điểm
        const data = {
            thumbnailId: thumbnailUrl,
        };

        let id = placeId;

        console.log('id', id);
        console.log('data', data);

        await updateThumbnailApi(id, thumbnailUrl);

        // Cập nhật thumbnail hiển thị
        thumbnail.value = thumbnailUrl;
        newThumbnailFile.value = null; // Reset file sau khi cập nhật thành công
        alert('Cập nhật ảnh đại diện thành công!');
    } catch (error) {
        console.error('Lỗi khi cập nhật thumbnail:', error);
        alert('Không thể cập nhật ảnh đại diện. Vui lòng thử lại.');
    }
};
  
  // Lưu thông tin địa điểm
  const saveLocation = async () => {
    if (!newLocation.value.name || !newLocation.value.url) {
      alert('Vui lòng nhập đầy đủ tên và đường dẫn.');
      return;
    }
    try {
      const data = {
        ...newLocation.value,
        thumbnail: thumbnail.value,
      };
      await updatePlaceApi(placeId, data);
      alert('Cập nhật thông tin địa điểm thành công!');
    } catch (error) {
      console.error('Lỗi khi cập nhật địa điểm:', error);
      alert('Không thể cập nhật địa điểm.');
    }
  };
  
  // Hủy form
  const cancelForm = () => {
    fetchPlace();
    newThumbnailFile.value = null; // Reset file thumbnail nếu có
  };
  
  // Tải dữ liệu khi component được mount
  onMounted(() => {
    fetchPlace();
  });
  </script>
  
  <style scoped>
  .card {
    border: none;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
    overflow: hidden;
    background: #ffffff;
  }
  
  .card-header {
    background: #f8f9fa;
    border-bottom: 1px solid #e9ecef;
    padding: 15px 20px;
  }
  
  .card-header h5 {
    margin: 0;
    font-size: 1.5rem;
    font-weight: 600;
    color: #333;
  }
  
  .card-body {
    padding: 30px;
  }
  
  .form-section {
    padding: 20px;
    background: #f8f9fa;
    border-radius: 8px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  }
  
  .section-title {
    font-size: 1.2rem;
    font-weight: 600;
    color: #333;
    margin-bottom: 20px;
  }
  
  .form-group {
    margin-bottom: 20px;
  }
  
  .form-group label {
    font-size: 0.95rem;
    font-weight: 500;
    color: #333;
    margin-bottom: 8px;
    display: block;
  }
  
  .required {
    color: #dc3545;
  }
  
  .form-control {
    width: 100%;
    padding: 12px;
    border: 1px solid #ced4da;
    border-radius: 6px;
    font-size: 0.95rem;
    transition: border-color 0.3s ease, box-shadow 0.3s ease;
  }
  
  .form-control:focus {
    border-color: #007bff;
    box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.1);
    outline: none;
  }
  
  .form-control[readonly] {
    background: #e9ecef;
    cursor: not-allowed;
  }
  
  .form-control-file {
    padding: 10px;
    border: 1px solid #ced4da;
    border-radius: 6px;
    background: #fff;
  }
  
  textarea.form-control {
    resize: none;
    height: 120px;
  }
  
  .thumbnail-preview {
    margin-top: 15px;
  }
  
  .thumbnail-preview h6 {
    font-size: 0.95rem;
    font-weight: 500;
    color: #333;
    margin-bottom: 10px;
  }
  
  .thumbnail-preview img {
    max-width: 100%;
    max-height: 200px;
    border-radius: 8px;
    object-fit: cover;
    border: 1px solid #dee2e6;
  }
  
  .form-actions {
    display: flex;
    gap: 10px;
    margin-top: 20px;
  }
  
  .btn {
    padding: 12px 20px;
    font-size: 0.95rem;
    font-weight: 500;
    border-radius: 6px;
    transition: all 0.3s ease;
    flex: 1;
  }
  
  .btn-success {
    background: #28a745;
    border: none;
    color: white;
  }
  
  .btn-success:hover {
    background: #218838;
  }
  
  .btn-primary {
    background: #007bff;
    border: none;
    color: white;
  }
  
  .btn-primary:hover {
    background: #0056b3;
  }
  
  .btn-primary:disabled {
    background: #6c757d;
    cursor: not-allowed;
  }
  
  .btn-secondary {
    background: #6c757d;
    border: none;
    color: white;
  }
  
  .btn-secondary:hover {
    background: #5a6268;
  }
  
  .btn i {
    margin-right: 8px;
  }
  </style>