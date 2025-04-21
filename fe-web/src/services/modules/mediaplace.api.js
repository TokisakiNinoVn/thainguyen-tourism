import instance from '@/services/axiosConfig';

const getListMediaPlaceApi = async (id) => instance.get(`/api/admin/place-media/all/${id}`);

export {
    getListMediaPlaceApi
};