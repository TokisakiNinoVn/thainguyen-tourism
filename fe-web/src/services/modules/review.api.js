import instance from '@/services/axiosConfig';

const createReviewApi = async (data) => instance.post('/api/web/review/create', data);

const getReviewPlaceByIdApi = async (id) => instance.get(`/api/web/place/review/${id}`);

export { createReviewApi, getReviewPlaceByIdApi };