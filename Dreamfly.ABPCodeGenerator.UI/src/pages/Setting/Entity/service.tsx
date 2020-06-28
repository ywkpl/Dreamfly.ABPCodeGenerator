import request from '@/utils/request';
import appConsts from '@/utils/AppConsts';

export async function generatorCode(params: any) {
  return request(`${appConsts.apiUrl}/entity/generate`, {
    method: 'POST',
    data: params,
  });
}

export async function removeCode(params: any) {
  return request(`${appConsts.apiUrl}/entity/remove`, {
    method: 'POST',
    data: params,
  });
}
