import instance from '@/apis/axiosConfig';

const getAllReviewApi = async () => instance.get('/api/admin/review/all');

const postApproveReviewApi = async (id) => instance.post(`/api/admin/review/approve/${id}`);

// delete
const deleteReviewApi = async (id) => instance.delete(`/api/admin/review/delete/${id}`);


export {
    getAllReviewApi,
    postApproveReviewApi,
    deleteReviewApi
};