import instance from '@/apis/axiosConfig';

const getListMediaPlaceApi = async (id) => instance.get(`/api/admin/place-media/all/${id}`);

// delete media place
const deleteMediaPlaceApi = async (id) => instance.delete(`/api/admin/place-media/delete/${id}`);

export {
    getListMediaPlaceApi,
    deleteMediaPlaceApi
};