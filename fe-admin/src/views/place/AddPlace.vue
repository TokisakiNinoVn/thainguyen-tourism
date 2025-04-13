<template>
  <div class="card">
    <div class="card-header">
      <h6>Thêm địa điểm</h6>
    </div>
    <div class="card-body">
      <div class="container-fluid">
        <div class="row">
          <!-- Bản đồ bên trái -->
          <div class="col-md-8 map-section">
            <div class="map-container">
              <div id="map" ref="mapRef"></div>
              <div class="map-controls">
                <button class="btn btn-primary" @click="resetMap">
                  <i class="fas fa-undo"></i> Reset bản đồ
                </button>
              </div>
            </div>
          </div>
          <!-- Danh sách địa điểm bên phải -->
          <div class="col-md-4 location-section">
            <div class="location-list">
              <div class="location-header">
                <h5>Danh sách địa điểm</h5>
                <div class="location-actions">
                  <button class="btn btn-success btn-sm" @click="openMapClick">
                    <i class="fas fa-plus"></i> Thêm điểm
                  </button>
                  <button class="btn btn-info btn-sm" @click="showCoordsForm = true">
                    <i class="fas fa-map-pin"></i> Nhập tọa độ
                  </button>
                </div>
              </div>
              <div class="location-scroll">
                <div class="list-group">
                  <button
                    v-for="location in locations"
                    :key="location.id"
                    class="list-group-item"
                    :class="{ active: selectedLocation?.id === location.id }"
                    @click="highlightLocation(location)"
                  >
                    <div class="location-info">
                      <img :src="instance.defaults.baseURL + location.imageUrl" alt="Thumbnail" class="img-thumbnail" />
                      <div class="more-infor-place">
                        <h6>{{ location.name }}</h6>
                        <p>{{ location.description || 'Không có mô tả' }}</p>
                        <small>Tọa độ: {{ location.coords.join(', ') }}</small>
                      </div>
                    </div>
                  </button>
                  <div v-if="!locations.length" class="list-group-item text-center">
                    Chưa có địa điểm nào
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- Form thêm địa điểm bằng click -->
      <div class="modal" v-if="showForm">
        <div class="modal-content">
          <div class="modal-header">
            <h5><i class="fas fa-map-marker-alt"></i> Thêm địa điểm mới</h5>
            <button class="close-btn" @click="cancelForm"><i class="fas fa-times"></i></button>
          </div>
          <div class="modal-body">
            <div class="form-group coords-group">
              <label>Vĩ độ</label>
              <input :value="newLocation.latitude" readonly />
            </div>
            <div class="form-group coords-group">
              <label>Kinh độ</label>
              <input :value="newLocation.longitude" readonly />
            </div>
            <div class="form-group">
              <label>Tên địa điểm</label>
              <input v-model="newLocation.name" type="text" placeholder="Nhập tên địa điểm" required />
            </div>
            <div class="form-group">
              <label>Mô tả</label>
              <textarea v-model="newLocation.description" placeholder="Nhập mô tả"></textarea>
            </div>
            <div class="form-group">
              <label>Đường dẫn</label>
              <input v-model="newLocation.url" type="url" placeholder="Nhập URL" required />
            </div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-success" @click="saveLocation">
              <i class="fas fa-save"></i> Lưu
            </button>
            <button class="btn btn-secondary" @click="cancelForm">
              <i class="fas fa-times"></i> Hủy
            </button>
          </div>
        </div>
      </div>
      <!-- Form thêm địa điểm bằng tọa độ -->
      <div class="modal" v-if="showCoordsForm">
        <div class="modal-content">
          <div class="modal-header">
            <h5><i class="fas fa-map-pin"></i> Nhập tọa độ</h5>
            <button class="close-btn" @click="showCoordsForm = false"><i class="fas fa-times"></i></button>
          </div>
          <div class="modal-body">
            <div class="form-group coords-group">
              <label>Vĩ độ</label>
              <div class="input-group">
                <input v-model="newLocation.latitude" type="number" step="any" placeholder="Nhập vĩ độ" required />
                <button class="paste-btn" @click="pasteCoords('latitude')"><i class="fas fa-paste"></i></button>
              </div>
            </div>
            <div class="form-group coords-group">
              <label>Kinh độ</label>
              <div class="input-group">
                <input v-model="newLocation.longitude" type="number" step="any" placeholder="Nhập kinh độ" required />
                <button class="paste-btn" @click="pasteCoords('longitude')"><i class="fas fa-paste"></i></button>
              </div>
            </div>
            <div class="form-group">
              <label>Tên địa điểm</label>
              <input v-model="newLocation.name" type="text" placeholder="Nhập tên địa điểm" required />
            </div>
            <div class="form-group">
              <label>Mô tả</label>
              <textarea v-model="newLocation.description" placeholder="Nhập mô tả"></textarea>
            </div>
            <div class="form-group">
              <label>Đường dẫn</label>
              <input v-model="newLocation.url" type="url" placeholder="Nhập URL" required />
            </div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-success" @click="saveLocationByCoords">
              <i class="fas fa-save"></i> Lưu
            </button>
            <button class="btn btn-secondary" @click="showCoordsForm = false">
              <i class="fas fa-times"></i> Hủy
            </button>
          </div>
        </div>
      </div>
      <!-- Popup thông tin -->
      <div class="info-panel" v-if="showInfo">
        <div class="info-header">
          <h5><i class="fas fa-info-circle"></i> {{ selectedLocation.name }}</h5>
          <button class="close-btn" @click="showInfo = false"><i class="fas fa-times"></i></button>
        </div>
        <div class="info-body">
          <p><i class="fas fa-align-left"></i> <strong>Mô tả:</strong> {{ selectedLocation.description || 'Không có' }}</p>
          <p><i class="fas fa-map-pin"></i> <strong>Tọa độ:</strong> {{ selectedLocation.coords?.join(', ') }}</p>
        </div>
        <div class="info-footer">
          <button class="btn btn-primary" @click="openUrl">
            <i class="fas fa-eye"></i> Xem chi tiết
          </button>
          <button class="btn btn-secondary" @click="showInfo = false">
            <i class="fas fa-times"></i> Đóng
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { createPlaceApi, getListPlaceApi } from '@/apis/modules/place.api';
import instance from '@/apis/axiosConfig';

// Biến reactive
const map = ref(null);
const mapRef = ref(null);
const thaiNguyenBounds = ref(null);
const locations = ref([]);
const showForm = ref(false);
const showCoordsForm = ref(false);
const showInfo = ref(false);
const tempLatLng = ref(null);
const initialMapState = ref({ center: [21.6, 105.85], zoom: 10 });
const newLocation = ref({
  name: '',
  description: '',
  url: '',
  latitude: null,
  longitude: null,
  coords: null,
  imageUrl: '',
});
const selectedLocation = ref(null);
const markers = ref({});

// Khởi tạo bản đồ
const initializeMap = () => {
  if (!mapRef.value) return;
  map.value = L.map(mapRef.value, {
    center: initialMapState.value.center,
    zoom: initialMapState.value.zoom,
    zoomAnimation: false,
  });
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap contributors',
  }).addTo(map.value);

  fetch(`${instance.defaults.baseURL}/files/ThaiNguyen.geojson`)
    .then((response) => {
      if (!response.ok) throw new Error('Không tìm thấy GeoJSON');
      return response.json();
    })
    .then((data) => {
      const thaiNguyenLayer = L.geoJSON(data, {
        style: {
          color: '#ff7800',
          weight: 2,
          fillColor: '#00FF00',
          fillOpacity: 0.3,
        },
      }).addTo(map.value);
      map.value.fitBounds(thaiNguyenLayer.getBounds());
      thaiNguyenBounds.value = thaiNguyenLayer.getBounds();
    })
    .catch((error) => {
      console.error('Lỗi khi tải GeoJSON:', error);
      alert('Không thể tải bản đồ tỉnh Thái Nguyên.');
    });
};

// Reset bản đồ về trạng thái ban đầu
const resetMap = () => {
  if (map.value) {
    map.value.setView(initialMapState.value.center, initialMapState.value.zoom, { animate: false });
  }
};

// Load danh sách địa điểm từ API
const loadLocations = async () => {
  try {
    const response = await getListPlaceApi();
    if (response.code === 200) {
      locations.value = response.data.map((place) => ({
        id: place.id,
        name: place.name,
        description: place.description,
        coords: [place.latitude, place.longitude],
        url: place.thumbnail || '',
        thumbnail: place.thumbnail || 'https://i.pinimg.com/736x/98/2e/5a/982e5a064861811e22abcf0e76373efd.jpg',
      }));
      locations.value.forEach((location) => addMarker(location));
    }
  } catch (error) {
    console.error('Lỗi khi tải danh sách địa điểm:', error);
    alert('Không thể tải danh sách địa điểm.');
  }
};

// Thêm marker vào bản đồ
const addMarker = (location) => {
  if (!map.value) return;
  const customIcon = L.divIcon({
    className: 'custom-marker',
    html: `
      <div class="marker-wrapper">
        <img src="${location.thumbnail}" 
            class="marker-image" 
            style="width: 40px; 
                    height: 40px; 
                    border-radius: 50%; 
                    object-fit: cover; 
                    border: 2px solid #fff; 
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2); 
                    position: absolute; 
                    top: -12px;" 
            onmouseover="this.style.border='2px solid green';" 
            onmouseout="this.style.border='2px solid #fff';" />
        <span class="marker-pin">📍</span>
    </div>
    `,
    iconSize: [40, 40],
    iconAnchor: [20, 40],
    popupAnchor: [0, -40],
  });
  const marker = L.marker(location.coords, { icon: customIcon }).addTo(map.value);
  marker.bindPopup(`<b>${location.name}</b><br>${location.description || 'Không có mô tả'}`);
  marker.on('mouseover', () => marker.openPopup());
  marker.on('mouseout', () => marker.closePopup());
  marker.on('click', () => {
    selectedLocation.value = location;
    showInfo.value = true;
  });
  markers.value[location.id] = marker;
};

// Mở chế độ click để chọn điểm
const openMapClick = () => {
  if (!map.value) return;
  map.value.on('click', (e) => {
    tempLatLng.value = e.latlng;
    if (thaiNguyenBounds.value && thaiNguyenBounds.value.contains(tempLatLng.value)) {
      newLocation.value.latitude = tempLatLng.value.lat;
      newLocation.value.longitude = tempLatLng.value.lng;
      newLocation.value.coords = [tempLatLng.value.lat, tempLatLng.value.lng];
      showForm.value = true;
    } else {
      alert('Vui lòng chọn điểm trong phạm vi tỉnh Thái Nguyên!');
    }
    map.value.off('click');
  });
};

// Dán tọa độ từ clipboard
const pasteCoords = async (field) => {
  try {
    const text = await navigator.clipboard.readText();
    if (field === 'latitude') {
      newLocation.value.latitude = parseFloat(text);
    } else if (field === 'longitude') {
      newLocation.value.longitude = parseFloat(text);
    }
  } catch (error) {
    console.error('Lỗi khi dán tọa độ:', error);
    alert('Không thể dán tọa độ từ clipboard.');
  }
};

// Lưu địa điểm
const saveLocation = async () => {
  if (!newLocation.value.name || !newLocation.value.url || !newLocation.value.latitude || !newLocation.value.longitude) {
    alert('Vui lòng nhập đầy đủ tên, đường dẫn và tọa độ.');
    return;
  }
  try {
    const payload = {
      name: newLocation.value.name,
      description: newLocation.value.description,
      url: newLocation.value.url,
      latitude: newLocation.value.latitude,
      longitude: newLocation.value.longitude,
    };
    const response = await createPlaceApi(payload);
    const newPlace = {
      id: response.data?.id || Date.now(),
      name: payload.name,
      description: payload.description,
      coords: [payload.latitude, payload.longitude],
      url: payload.url,
      thumbnail: payload.url || 'https://i.pinimg.com/736x/98/2e/5a/982e5a064861811e22abcf0e76373efd.jpg',
    };
    locations.value.push(newPlace);
    addMarker(newPlace);
    showForm.value = false;
    newLocation.value = { name: '', description: '', url: '', latitude: null, longitude: null, coords: null };
  } catch (error) {
    console.error('Lỗi khi tạo địa điểm:', error);
    alert('Không thể tạo địa điểm. Vui lòng thử lại.');
  }
};

// Lưu địa điểm bằng tọa độ
const saveLocationByCoords = async () => {
  if (!newLocation.value.name || !newLocation.value.url || !newLocation.value.latitude || !newLocation.value.longitude) {
    alert('Vui lòng nhập đầy đủ tên, đường dẫn và tọa độ.');
    return;
  }
  const latLng = L.latLng(newLocation.value.latitude, newLocation.value.longitude);
  if (!thaiNguyenBounds.value.contains(latLng)) {
    alert('Tọa độ không nằm trong phạm vi tỉnh Thái Nguyên!');
    return;
  }
  try {
    const payload = {
      name: newLocation.value.name,
      description: newLocation.value.description,
      url: newLocation.value.url,
      latitude: newLocation.value.latitude,
      longitude: newLocation.value.longitude,
    };
    const response = await createPlaceApi(payload);
    const newPlace = {
      id: response.data?.id || Date.now(),
      name: payload.name,
      description: payload.description,
      coords: [payload.latitude, payload.longitude],
      url: payload.url,
      thumbnail: payload.url || 'https://i.pinimg.com/736x/98/2e/5a/982e5a064861811e22abcf0e76373efd.jpg',
    };
    locations.value.push(newPlace);
    addMarker(newPlace);
    showCoordsForm.value = false;
    newLocation.value = { name: '', description: '', url: '', latitude: null, longitude: null, coords: null };
  } catch (error) {
    console.error('Lỗi khi tạo địa điểm:', error);
    alert('Không thể tạo địa điểm. Vui lòng thử lại.');
  }
};

// Hủy form
const cancelForm = () => {
  showForm.value = false;
  newLocation.value = { name: '', description: '', url: '', latitude: null, longitude: null, coords: null };
};

// Mở URL
const openUrl = () => {
  if (selectedLocation.value.url) {
    window.open(selectedLocation.value.url, '_blank');
  }
};

// Highlight địa điểm
const highlightLocation = (location) => {
  if (!map.value) return;
  selectedLocation.value = location;
  const marker = markers.value[location.id];
  if (marker) {
    map.value.setView(location.coords, 13, { animate: false });
    marker.openPopup();
  }
};

// Cleanup khi component bị hủy
onBeforeUnmount(() => {
  if (map.value) {
    map.value.off();
    map.value.remove();
    map.value = null;
  }
});

// Khởi tạo
onMounted(() => {
  initializeMap();
  loadLocations();
});
</script>

<style scoped>
.card {
  border: none;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.card-header {
  background: #ffffff;
  border-bottom: 1px solid #e9ecef;
  padding: 15px 20px;
}

.card-header h6 {
  margin: 0;
  font-size: 1.25rem;
  font-weight: 600;
  color: #333;
}

.card-body {
  padding: 20px;
  background: #f8f9fa;
}

.map-section {
  padding-right: 10px;
}

.map-container {
  position: relative;
  height: 70vh;
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

#map {
  width: 100%;
  height: 100%;
}

.map-controls {
  position: absolute;
  top: 10px;
  right: 10px;
  z-index: 1000;
}

.map-controls .btn {
  padding: 8px 16px;
  font-size: 0.9rem;
  border-radius: 6px;
  transition: all 0.3s ease;
}

.location-section {
  padding-left: 10px;
}

.location-list {
  background: #ffffff;
  border-radius: 10px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  height: 80vh;
  display: flex;
  flex-direction: column;
}

.location-header {
  padding: 15px 20px;
  border-bottom: 1px solid #e9ecef;
  display: flex;
  justify-content: space-between;
  align-items: left;
  flex-direction: column;
}

.location-header h5 {
  margin: 0;
  font-size: 1.1rem;
  font-weight: 600;
  color: #333;
}

.location-actions {
  display: flex;
  gap: 10px;
}

.location-actions .btn {
  padding: 6px 12px;
  font-size: 0.85rem;
  border-radius: 6px;
}

.location-scroll {
  flex: 1;
  overflow-y: auto;
  padding: 10px 0;
}

.list-group-item {
  border: none;
  padding: 15px 20px;
  margin: 5px 10px;
  border-radius: 8px;
  background: #f8f9fa;
  transition: all 0.3s ease;
  cursor: pointer;
}

.list-group-item:hover {
  background: #e9ecef;
}

.list-group-item.active {
  background: #007bff;
  color: white;
}

.list-group-item.active h6,
.list-group-item.active p,
.list-group-item.active small {
  color: white;
}

.location-info {
  display: flex;
  align-items: center;
  text-align: left;
}
.img-thumbnail {
  width: 100px;
  border-radius: 50%;
  margin-right: 10px;
}

.location-info h6 {
  margin: 0 0 5px;
  font-size: 1rem;
  font-weight: 600;
}

.location-info p {
  margin: 0 0 5px;
  font-size: 0.9rem;
  color: #555;
}

.location-info small {
  font-size: 0.8rem;
  color: #777;
}

.modal {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2000;
}

.modal-content {
  background: white;
  border-radius: 12px;
  width: 400px;
  max-width: 90%;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
  overflow: hidden;
}

.modal-header {
  padding: 15px 20px;
  border-bottom: 1px solid #e9ecef;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-header h5 {
  margin: 0;
  font-size: 1.2rem;
  font-weight: 600;
  color: #333;
}

.modal-header h5 i {
  color: #28a745;
  margin-right: 8px;
}

.modal-header .close-btn {
  background: none;
  border: none;
  font-size: 1.2rem;
  color: #777;
  cursor: pointer;
}

.modal-body {
  padding: 20px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  font-size: 0.9rem;
  font-weight: 500;
  color: #333;
  margin-bottom: 5px;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 0.9rem;
  box-sizing: border-box;
  transition: border-color 0.3s ease;
}

.form-group input[readonly] {
  background: #f8f9fa;
  cursor: not-allowed;
}

.form-group input:focus,
.form-group textarea:focus {
  border-color: #007bff;
  outline: none;
}

.form-group textarea {
  height: 100px;
  resize: none;
}

.coords-group .input-group {
  display: flex;
  gap: 10px;
}

.coords-group input {
  flex: 1;
}

.coords-group .paste-btn {
  padding: 10px;
  background: #6c757d;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: background 0.3s ease;
}

.coords-group .paste-btn:hover {
  background: #5a6268;
}

.modal-footer {
  padding: 15px 20px;
  border-top: 1px solid #e9ecef;
  display: flex;
  gap: 10px;
}

.modal-footer .btn {
  flex: 1;
  padding: 10px;
  font-size: 0.9rem;
  border-radius: 6px;
}

.info-panel {
  position: fixed;
  top: 20px;
  right: 20px;
  background: white;
  border-radius: 12px;
  width: 320px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
  z-index: 1500;
  overflow: hidden;
}

.info-header {
  padding: 15px 20px;
  border-bottom: 1px solid #e9ecef;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #f8f9fa;
}

.info-header h5 {
  margin: 0;
  font-size: 1.1rem;
  font-weight: 600;
  color: #333;
}

.info-header h5 i {
  color: #28a745;
  margin-right: 8px;
}

.info-header .close-btn {
  background: none;
  border: none;
  font-size: 1.2rem;
  color: #777;
  cursor: pointer;
}

.info-body {
  padding: 20px;
}

.info-body p {
  margin: 10px 0;
  font-size: 0.9rem;
  color: #555;
  line-height: 1.5;
}

.info-body p i {
  color: #28a745;
  margin-right: 8px;
}

.info-footer {
  padding: 15px 20px;
  border-top: 1px solid #e9ecef;
  display: flex;
  gap: 10px;
}

.info-footer .btn {
  flex: 1;
  padding: 10px;
  font-size: 0.9rem;
  border-radius: 6px;
}

.btn-primary {
  background: #007bff;
  border: none;
}

.btn-primary:hover {
  background: #0056b3;
}

.btn-success {
  background: #28a745;
  border: none;
}

.btn-success:hover {
  background: #218838;
}

.btn-secondary {
  background: #6c757d;
  border: none;
}

.btn-secondary:hover {
  background: #5a6268;
}

.custom-marker {
  display: flex;
  justify-content: center;
  align-items: center;
}

.marker-wrapper {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
}

.marker-image {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  object-fit: cover;
  border: 2px solid #fff;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
  position: absolute;
  top: -12px;
}

.marker-pin {
  font-size: 24px;
  color: #ff0000;
  text-shadow: 0 2px 5px rgba(0, 0, 0, 0.3);
}
</style>