import { api } from './api';

const headers = {
  'Content-Type': 'application/json',
};

export const player = async (data) => await api.post(`/Players`, data, { headers: headers });
