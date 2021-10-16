import { api } from './api';

export const createBingoCard = async (id) => await api.post(`/BingoCards/${id}`);

export const fetchBingoCard = async (id) => await api.get(`/BingoCards/${id}`);
