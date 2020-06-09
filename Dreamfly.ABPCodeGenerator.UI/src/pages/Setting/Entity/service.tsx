import request from '@/utils/request';
import appConsts from '@/utils/AppConsts';

export async function generatorCode(params: any) {
  return request(`${appConsts.apiUrl}/entity`, {
    method: 'POST',
    data: params,
  });
}
