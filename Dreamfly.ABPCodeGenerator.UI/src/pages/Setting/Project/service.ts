import request from '@/utils/request';
import appConsts from '@/utils/AppConsts';

export async function getProject(params: any) {
  return request(`${appConsts.apiUrl}/project`, {
    method: 'GET',
    data: params,
  });
}

export async function updateProject(params: any) {
  return request(`${appConsts.apiUrl}/project`, {
    method: 'POST',
    data: params,
  });
}
