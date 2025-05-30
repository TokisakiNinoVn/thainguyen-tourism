import instance from '@/apis/axiosConfig';

const getListPlaceApi = async () => instance.get('/api/admin/place/list');
const getListPlaceProvinApi = async (province) => instance.get('/api/admin/place/province', { params: { province: province } });
// create place
const createPlaceApi = async (data) => instance.post('/api/admin/place/create', data);
// update place
const updatePlaceApi = async (id, data) => instance.put(`/api/admin/place/update/${id}`, data);
// delete place
const deletePlaceApi = async (id) => instance.delete(`/api/admin/place/delete/${id}`);
// get place by id
const getPlaceByIdApi = async (id) => instance.get(`/api/admin/place/${id}`);
//update thumbnail
const updateThumbnailApi = async (id, data) => instance.post(`/api/admin/place/update/thumbnail/${id}`, data);

export {
    getListPlaceApi,
    createPlaceApi,
    updatePlaceApi,
    deletePlaceApi,
    getPlaceByIdApi,
    updateThumbnailApi,
    getListPlaceProvinApi
};